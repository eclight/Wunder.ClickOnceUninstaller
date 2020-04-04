﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Wunder.ClickOnceUninstaller
{
    public class UninstallInfo
    {
        public const string UninstallRegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

        private UninstallInfo()
        {
        }

        public static UninstallInfo Find(string appName)
        {
            return GetInstalledApps().FirstOrDefault(x => x.DisplayName == appName);
        }

        public static IEnumerable<UninstallInfo> GetInstalledApps()
        {
            var uninstall = Registry.CurrentUser.OpenSubKey(UninstallRegistryPath);
            if (uninstall != null)
            {
                foreach (var app in uninstall.GetSubKeyNames())
                {
                    var sub = uninstall.OpenSubKey(app);

                    yield return new UninstallInfo
                    {
                        Key = app,
                        DisplayName = sub.GetValue("DisplayName") as string,
                        UninstallString = sub.GetValue("UninstallString") as string,
                        ShortcutFolderName = sub.GetValue("ShortcutFolderName") as string,
                        ShortcutSuiteName = sub.GetValue("ShortcutSuiteName") as string,
                        ShortcutFileName = sub.GetValue("ShortcutFileName") as string,
                        SupportShortcutFileName = sub.GetValue("SupportShortcutFileName") as string,
                        Version = sub.GetValue("DisplayVersion") as string
                    };
                }
            }
        }

        public string Key { get; set; }

        public string DisplayName { get; private set; }

        public string UninstallString { get; private set; }

        public string ShortcutFolderName { get; set; }

        public string ShortcutSuiteName { get; set; }

        public string ShortcutFileName { get; set; }

        public string SupportShortcutFileName { get; set; }

        public string Version { get; set; }

        public string GetPublicKeyToken()
        {
            var token = UninstallString.Split(',').First(s => s.Trim().StartsWith("PublicKeyToken=")).Substring(16);
            if (token.Length != 16) throw new ArgumentException();
            return token;
        }
    }
}
