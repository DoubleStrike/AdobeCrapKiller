using System;
using System.Diagnostics;

namespace AdobeCrapKiller
{
    public static class ProcessExtensions
    {
        public static bool IsRunning(string name) => Process.GetProcessesByName(name).Length > 0;

        public static Process[] GetByName(string name)
        {
            return Process.GetProcessesByName(name);
        }

        public static bool KillByName(string name)
        {
            try
            {
                Process[] ProcessList = GetByName(name);

                // Early return if empty
                if (ProcessList.Length == 0) return false;

                foreach (Process ProcessToKill in ProcessList)
                {
                    ProcessToKill.Kill();
                }

                return true;
            } catch
            {
                return false;
            }
        }
    }
}
