﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Wunder.ClickOnceUninstaller
{
    public class RemoveFiles : IUninstallStep
    {
        private string _clickOnceFolder;
        private string _clickOnceDataFolder;

        private List<string> _foldersToRemove;
        private List<string> _filesToRemove;

        public void Prepare(List<string> componentsToRemove)
        {
            _foldersToRemove = new List<string>();
            
            _clickOnceFolder = FindClickOnceFolder();
            foreach (var directory in Directory.GetDirectories(_clickOnceFolder))
            {
                if (componentsToRemove.Contains(Path.GetFileName(directory)))
                {
                    _foldersToRemove.Add(directory);
                }
            }

            _clickOnceDataFolder = FindClickOnceDataFolder();
            foreach (var directory in Directory.GetDirectories(_clickOnceDataFolder))
            {
                if (componentsToRemove.Contains(Path.GetFileName(directory)))
                {
                    _foldersToRemove.Add(directory);
                }
            }
            
            _filesToRemove = new List<string>();
            foreach (var file in Directory.GetFiles(Path.Combine(_clickOnceFolder, "manifests")))
            {
                if (componentsToRemove.Contains(Path.GetFileNameWithoutExtension(file)))
                {
                    _filesToRemove.Add(file);
                }
            }
        }

        public void PrintDebugInformation(TextWriter debugLog)
        {
            if (string.IsNullOrEmpty(_clickOnceFolder) || !Directory.Exists(_clickOnceFolder))
                throw new InvalidOperationException("Call Prepare() first.");

            debugLog.WriteLine("Remove files from " + _clickOnceFolder);
            debugLog.WriteLine("Remove files from " + _clickOnceDataFolder);

            foreach (var folder in _foldersToRemove)
            {
                debugLog.WriteLine("Delete folder " + folder.Substring(_clickOnceFolder.Length + 1));
            }

            foreach (var file in _filesToRemove)
            {
                debugLog.WriteLine("Delete file " + file.Substring(_clickOnceFolder.Length + 1));
            }

            debugLog.WriteLine();
        }

        public void Execute()
        {
            if (string.IsNullOrEmpty(_clickOnceFolder) || !Directory.Exists(_clickOnceFolder))
                throw new InvalidOperationException("Call Prepare() first.");

            foreach (var folder in _foldersToRemove)
            {
                try
                {
                    Directory.Delete(folder, true);
                }
                catch (UnauthorizedAccessException)
                {
                }
            }

            foreach (var file in _filesToRemove)
            {
                File.Delete(file);
            }
        }

        private string FindClickOnceFolder()
        {
            return DescendIntoSubfolders(Utils.GetClickonceRootFolder());
        }

        private string FindClickOnceDataFolder()
        {
            return DescendIntoSubfolders(Path.Combine(Utils.GetClickonceRootFolder(), "Data"));
        }

        private static string DescendIntoSubfolders(string baseFolder)
        {
            if (!Directory.Exists(baseFolder)) throw new ArgumentException("Could not find ClickOnce folder.");

            foreach (var subFolder in Directory.GetDirectories(baseFolder))
            {
                if ((Path.GetFileName(subFolder) ?? string.Empty).Length == 12)
                {
                    foreach (var subSubFolder in Directory.GetDirectories(subFolder))
                    {
                        if ((Path.GetFileName(subSubFolder) ?? string.Empty).Length == 12)
                        {
                            return subSubFolder;
                        }
                    }
                }
            }

            throw new ArgumentException("Could not find ClickOnce folder");
        }

        public void Dispose()
        {
        }
    }
}
