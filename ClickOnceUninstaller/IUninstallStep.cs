using System;
using System.IO;
using System.Collections.Generic;

namespace Wunder.ClickOnceUninstaller
{
    public interface IUninstallStep : IDisposable
    {
        void Prepare(List<string> componentsToRemove);

        void PrintDebugInformation(TextWriter debugLog);

        void Execute();
    }
}
