﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Windows.Documents;

namespace AdobeCrapKiller
{
    public static class ProcessExtensions
    {
        public static bool IsRunning(string name) => Process.GetProcessesByName(name).Length > 0;

        public static Process[] GetByName(string name)
        {
            return Process.GetProcessesByName(name);
        }

        public static List<Process> GetByPathSubstring(string pathComponent)
        {
            List<Process> processesToReturn = new List<Process>();

            // Cycle through all processes and do a string match
            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (p.MainModule.FileName.Contains(pathComponent, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!p.MainModule.FileName.Contains("killer", StringComparison.InvariantCultureIgnoreCase)) {
                            processesToReturn.Add(p);
                        }
                    }
                } catch (Exception) { }
            }

            return processesToReturn;
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
                    SystemSounds.Beep.Play();
                }

                return true;
            } catch
            {
                return false;
            }
        }
    }
}
