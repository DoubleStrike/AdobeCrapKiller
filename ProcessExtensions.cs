using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Windows;

namespace AdobeCrapKiller {
    public static class ProcessExtensions {
        public static bool IsRunning(string name) => Process.GetProcessesByName(name).Length > 0;

        /// <summary>
        /// Retrieves all processes on the local computer that have the specified name.
        /// </summary>
        /// <remarks>This method only returns processes running on the local computer. To retrieve
        /// processes on a remote computer, use <see cref="Process.GetProcessesByName(string, string)"/>. The returned
        /// array may be empty if no processes with the specified name are running.</remarks>
        /// <param name="name">The case-insensitive name of the process to search for. This should not include the file extension (for
        /// example, use "notepad" instead of "notepad.exe").</param>
        /// <returns>An array of <see cref="Process"/> objects representing all processes with the specified name. If no matching
        /// processes are found, the array is empty.</returns>
        public static Process[] GetByName(string name) {
            return Process.GetProcessesByName(name);
        }

        /// <summary>
        /// Retrieves a list of processes whose main module file path contains the specified substring, performing a
        /// case-insensitive search.
        /// </summary>
        /// <remarks>Processes that cannot be accessed due to insufficient permissions or system
        /// restrictions are skipped. The current process is excluded from the results.</remarks>
        /// <param name="pathComponent">The substring to search for within the file paths of process main modules. The comparison is
        /// case-insensitive.</param>
        /// <returns>A list of <see cref="Process"/> objects representing processes whose main module file path contains the
        /// specified substring. The list will be empty if no matching processes are found.</returns>
        public static List<Process> GetByPathSubstring(string pathComponent) {
            Logger.Log("GetByPathSubstring(): Searching for processes with path component: " + pathComponent);
            List<Process> processesToReturn = new();

            // Cycle through all processes and do a string match
            foreach (Process p in Process.GetProcesses()) {
                try {
                    Logger.Log($"GetByPathSubstring(): Checking process: {p.ProcessName}");
                    if (p.MainModule == null || !p.MainModule.FileName.Contains(pathComponent, StringComparison.InvariantCultureIgnoreCase)) {
                        continue;
                    }

                    // Exclude self
                    if (!p.MainModule.FileName.Contains("killer", StringComparison.InvariantCultureIgnoreCase)) {
                        processesToReturn.Add(p);
                    }
                } catch (System.ComponentModel.Win32Exception e) {
                    Logger.Log($"  GetByPathSubstring():Error reading process {p.ProcessName}: {e.Message}");
                } catch (Exception) { }
            }

            Logger.Log($"GetByPathSubstring(): Found {processesToReturn.Count} matching processes.");
            return processesToReturn;
        }

        /// <summary>
        /// Attempts to terminate the process with the specified identifier and its child processes.
        /// </summary>
        /// <remarks>This method attempts to kill both the specified process and any child processes it
        /// has started. If the process does not exist or cannot be terminated, the method returns false. The caller
        /// must have sufficient privileges to terminate the target process.</remarks>
        /// <param name="Id">The unique identifier of the process to terminate. Must correspond to an active process.</param>
        /// <returns>true if the process and its child processes were successfully terminated; otherwise, false.</returns>
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
                return false;
            }

            Logger.Log("KillById(): Successfully killed process with ID: " + Id);
            return true;
        }
    }
}
