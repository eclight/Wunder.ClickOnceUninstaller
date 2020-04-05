using System;
using System.IO;

namespace Wunder.ClickOnceUninstaller
{
    public static class Utils
    {
        public static string GetClickonceRootFolder()
        {
            return IsWindowsXp()
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"..\Apps\2.0")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Apps\2.0");
        }

        private static bool IsWindowsXp()
        {
            return Environment.OSVersion.Version.Major == 5;
        }
    }
}
