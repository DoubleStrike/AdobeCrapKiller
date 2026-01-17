using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Windows;

namespace AdobeCrapKiller {
    /// <summary>
    /// Provides extension methods for querying and managing system processes on the local computer.
    /// </summary>
    /// <remarks>This static class offers utility methods for retrieving, searching, and terminating processes
    /// by name, identifier, or file path. All methods operate on processes running on the local machine and may require
    /// elevated permissions to access or terminate certain processes. Some methods are marked as obsolete and should be
    /// replaced with their recommended alternatives.</remarks>
    public static class ProcessExtensions {
        public static bool IsRunning(string name) => Process.GetProcessesByName(name).Length > 0;

        /// <summary>
        /// Searches for running processes whose main module file path contains any of the specified substrings,
        /// excluding processes whose path contains "killer".
        /// </summary>
        /// <remarks>Processes whose main module file path contains "killer" (case-insensitive) are
        /// excluded from the results. Accessing process information may fail for some processes due to insufficient
        /// permissions or system restrictions; such processes are silently skipped.</remarks>
        /// <param name="pathComponents">A collection of substrings to match against the file paths of running processes. Each substring is compared
        /// case-insensitively. If any substring is found within a process's main module file path, that process is
        /// included in the results.</param>
        /// <returns>A list of <see cref="Process"/> objects representing processes whose main module file path contains at least
        /// one of the specified substrings. The list is empty if no matching processes are found.</returns>
        public static List<Process> GetByPathSubstrings(IReadOnlyCollection<string> pathComponents) {
            Logger.Log("GetByPathSubstrings(): Searching for processes with path components: " + string.Join(", ", pathComponents), LogLevel.Info);
            List<Process> processesToReturn = new();

            foreach (Process p in Process.GetProcesses()) {
                try {
                    Logger.Log($"GetByPathSubstrings(): Checking process: {p.ProcessName}", LogLevel.Info);

                    var module = p.MainModule;
                    if (module == null)
                        continue;

                    string fileName = module.FileName;

                    // Exclude self early
                    if (fileName.Contains("killer", StringComparison.OrdinalIgnoreCase))
                        continue;

                    // Match ANY provided substring
                    if (pathComponents.Any(pc => fileName.Contains(pc, StringComparison.OrdinalIgnoreCase))) {
                        processesToReturn.Add(p);
                    }
                } catch (System.ComponentModel.Win32Exception e) {
                    Logger.Log($"  GetByPathSubstrings(): Error reading process {p.ProcessName}: {e.Message}", LogLevel.Warning);
                } catch (Exception) { }
            }

            Logger.Log($"GetByPathSubstrings(): Found {processesToReturn.Count} matching processes.", LogLevel.Info);

            // Sort by Process.Id descending (highest Id first)
            processesToReturn.Sort((a, b) => b.Id.CompareTo(a.Id));
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
            Logger.Log("KillById(): Attempting to kill processes with ID: " + Id, LogLevel.Info);

            try {
                Process kp = Process.GetProcessById(Id);

                if (kp == null || kp.MainModule == null || kp.MainModule.FileName == null) {
                    Logger.Log("KillById(): No process found with ID: " + Id, LogLevel.Info);
                    return false;
                }

                // Kill process and subtree
                kp.Kill(true);
            } catch {
                Logger.Log("KillById(): Failed to kill process with ID: " + Id, LogLevel.Warning);
                return false;
            }

            Logger.Log("KillById(): Successfully killed process with ID: " + Id, LogLevel.Info);
            return true;
        }
    }
}
