using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Windows;

namespace AdobeCrapKiller {
    public static class ProcessExtensions {
        public static bool IsRunning(string name) => Process.GetProcessesByName(name).Length > 0;

        public static Process[] GetByName(string name) {
            return Process.GetProcessesByName(name);
        }

        public static List<Process> GetByPathSubstring(string pathComponent) {
            Logger.Log("GetByPathSubstring({pathComponent}): Searching for processes with path component: " + pathComponent);
            List<Process> processesToReturn = new();

            // Cycle through all processes and do a string match
            foreach (Process p in Process.GetProcesses()) {
                try {
                    Logger.Log($"GetByPathSubstring({pathComponent}): Checking process: {p.ProcessName}");
                    if (p.MainModule == null || !p.MainModule.FileName.Contains(pathComponent, StringComparison.InvariantCultureIgnoreCase)) {
                        continue;
                    }

                    if (!p.MainModule.FileName.Contains("killer", StringComparison.InvariantCultureIgnoreCase)) {
                        processesToReturn.Add(p);
                    }
                } catch (System.ComponentModel.Win32Exception e) {
                    Logger.Log($"  GetByPathSubstring({pathComponent}):Error reading process {p.ProcessName}: {e.Message}");
                } catch (Exception) { }
            }

            Logger.Log($"GetByPathSubstring({pathComponent}): Found {processesToReturn.Count} matching processes.");
            return processesToReturn;
        }

        public static bool KillByName(string name) {
            Logger.Log("KillByName({name}): Attempting to kill processes with name: " + name);

            try {
                Process[] ProcessList = GetByName(name);

                // Early return if empty
                if (ProcessList.Length == 0) return false;

                foreach (Process ProcessToKill in ProcessList) {
                    ProcessToKill.Kill();
                    SystemSounds.Beep.Play();
                }

                Logger.Log("KillByName({name}): Successfully killed processes with name: " + name);
                return true;
            } catch {
                Logger.Log("KillByName({name}): Failed to kill processes with name: " + name);
                return false;
            }
        }

        public static bool KillByPath(string path) {
            Logger.Log("KillByPath({path}): Attempting to kill processes with path: " + path);
            // TODO: Can we do this via a stored process list instead of re-querying all processes? If so, we will need to store a process ID in the list of process objects.
            foreach (Process kp in Process.GetProcesses()) {
                try {
                    if (kp == null || kp.MainModule == null || kp.MainModule.FileName == null) continue;

                    if (kp.MainModule != null && kp.MainModule.FileName.Equals(path, StringComparison.InvariantCultureIgnoreCase)) {
                        // Kill process and subtree
                        kp.Kill(true);
                    }
                } catch { 
                    Logger.Log($"KillByPath(): Failed to kill process: {kp.ProcessName}");
                }

                Logger.Log($"KillByPath(): Successfully killed process: {kp.ProcessName}");
            }

            return true;
        }

        public static bool KillById(int Id) {
            Logger.Log("KillById(): Attempting to kill processes with ID: " + Id);

            try {
                Process kp = Process.GetProcessById(Id);

                if (kp == null || kp.MainModule == null || kp.MainModule.FileName == null) {
                    Logger.Log("KillById(): No process found with ID: " + Id);
                    return false;
                }

                // Kill process and subtree
                kp.Kill(true);
            } catch {
                Logger.Log("KillById(): Failed to kill process with ID: " + Id);
            }

            Logger.Log("KillById(): Successfully killed process with ID: " + Id);

            return true;
        }
    }
}
