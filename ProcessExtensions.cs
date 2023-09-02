using System;
using System.Diagnostics;

namespace AdobeCrapKiller
{
    public static class ProcessExtensions
    {
        public static bool IsRunning(string name) => Process.GetProcessesByName(name).Length > 0;
    }
}
