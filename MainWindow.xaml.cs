#define CODEPATHNEW

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;

namespace AdobeCrapKiller {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        // Change the property to be nullable to satisfy CS8618
        ObservableCollection<AdobeMemoryWastingCrap>? processesToKill { get; set; }
        private System.Windows.Threading.DispatcherTimer getProcessStatusTimer = new();

        public MainWindow() {
            Logger.Log("MainWindow(): Initializing main window.");
            InitializeComponent();

            // Restart program and run as admin
            if (!IsAdministrator()) {
                Logger.Log("MainWindow(): Not running as admin, requesting elevation.");
                if (System.Diagnostics.Process.GetCurrentProcess().MainModule is ProcessModule exeModule) {
                    string elevateTitle = "Restart program as Administrator?";
                    string elevateMessage = "Admin privileges are required to kill 100% of all Adobe crap. \n \nSelect yes to run this app as admin. \nSelect no to try running as-is.";
                    if (MessageBox.Show(elevateMessage, elevateTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                        string exeName = SafeString(exeModule.FileName);
                        ProcessStartInfo startInfo = new(exeName) {
                            Verb = "runas",
                            UseShellExecute = true
                        };
                        Logger.Log("MainWindow(): Restarting program as admin: " + exeName);
                        System.Diagnostics.Process.Start(startInfo);

                        Logger.Log("MainWindow(): Exiting current non-admin instance.");
                        Application.Current.Shutdown();
                        return;
                    } else {
                        Logger.Log("MainWindow(): User chose to continue without admin privileges.");
                    }
                }
            } else {
                Logger.Log("MainWindow(): Running as admin.");

                // Mark title as red if we are running as admin, to make it more clear
                lblTitle.Foreground = new SolidColorBrush(Colors.Red);
                lblTitle.Content += " (Admin Mode)";
            }

            // Show version in titlebar
            Version? version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null) {
                this.Title += " v";
                this.Title += version.ToString();
            }

            // Setup auto-refresh timer properties
            Logger.Log("MainWindow(): Setting up auto-refresh timer.");
            getProcessStatusTimer.Tick += new EventHandler(getProcessStatusTimer_Tick);
            getProcessStatusTimer.Interval = new TimeSpan(0, 0, 0, 15);

            // Setup datagrid binding
            Logger.Log("MainWindow(): Setting up datagrid binding.");
            processesToKill = new ObservableCollection<AdobeMemoryWastingCrap>();
            dataGrid.ItemsSource = processesToKill;

            Logger.Log("MainWindow(): Main window initialization completed.");
        }

        #region Event handlers
        private void btnAutoRefresh_Click(object sender, RoutedEventArgs e) {
            Logger.Log("btnAutoRefresh_Click(): Auto-refresh started.");

            // Toggle button states
            btnAutoRefresh.IsEnabled = !btnAutoRefresh.IsEnabled;
            btnStop.IsEnabled = !btnStop.IsEnabled;

            // Start auto-refresh timer to show process state
            getProcessStatusTimer.Start();
        }

        private void btnKill_Click(object sender, RoutedEventArgs e) {
            Logger.Log("btnKill_Click(): Killing Adobe processes.");

            if (processesToKill == null) return;

            foreach (var item in processesToKill) {
                Logger.Log($"btnKill_Click(): Killing process at path: '{item.ProcessPath}' (ID: {item.ProcessId})");
                ProcessExtensions.KillById(item.ProcessId);
            }

            PopulateGrid();
            SystemSounds.Beep.Play();
            Logger.Log("btnKill_Click(): Finished killing Adobe processes.");
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            Logger.Log("btnRefresh_Click(): Manual refresh triggered.");
            PopulateGrid();
            Logger.Log("btnRefresh_Click(): Manual refresh completed.");
        }

        private void btnStop_Click(object sender, RoutedEventArgs e) {
            Logger.Log("btnStop_Click(): Auto-refresh stopped.");

            // Stop auto-refresh timer to show process state
            getProcessStatusTimer.Stop();

            // Toggle button states
            btnAutoRefresh.IsEnabled = !btnAutoRefresh.IsEnabled;
            btnStop.IsEnabled = !btnStop.IsEnabled;
        }

        private void getProcessStatusTimer_Tick(object? sender, EventArgs e) {
            Logger.Log("getProcessStatusTimer_Tick(): Auto-refresh tick triggered.");
            //SystemSounds.Beep.Play();
            PopulateGrid();
            Logger.Log("getProcessStatusTimer_Tick(): Auto-refresh tick completed.");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            Logger.Log("Window_Loaded(): Window loaded event triggered.");

            // Do the first refresh
            PopulateGrid();

            Logger.Log("Window_Loaded(): Initial process list population completed.");
        }
        #endregion

        #region Local methods
        /// <summary>
        /// Populates the process list grid with currently running Adobe and Acrobat processes targeted for termination.
        /// </summary>
        /// <remarks>This method refreshes the internal list of processes to be killed by scanning for
        /// processes whose executable paths contain "adobe" or "acrobat". The process list is cleared and repopulated
        /// each time this method is called. This method does not terminate any processes; it only updates the list for
        /// subsequent actions.</remarks>
        private void PopulateGrid() {
            // TODO: This is inefficient - build a process list once and filter from that instead of querying twice
            Logger.Log("PopulateGrid(): Populating process list grid.");
#if CODEPATHOLD
            List<Process> newProcessesAdobe = ProcessExtensions.GetByPathSubstring("adobe");
            Logger.Log("PopulateGrid(): Found " + newProcessesAdobe.Count + " Adobe-related processes.");
            List<Process> newProcessesAcrobat = ProcessExtensions.GetByPathSubstring("acrobat");
            Logger.Log("PopulateGrid(): Found " + newProcessesAcrobat.Count + " Acrobat-related processes.");
#else
            // New code for testing
            List<Process> newProcessesBoth = ProcessExtensions.GetByPathSubstrings(new[] { "adobe", "acrobat" });
            Logger.Log("PopulateGrid(): NEW CODE: Found " + newProcessesBoth.Count + " Adobe/Acrobat-related processes.");
#endif

            processesToKill ??= new();

            processesToKill.Clear();

#if CODEPATHOLD
            foreach (Process p in newProcessesAdobe) {
                if (p.MainModule?.FileName != null) {
                    processesToKill.Add(new AdobeMemoryWastingCrap(p.MainModule.FileName, p.Id));
                    Logger.Log($"PopulateGrid(): Added Adobe process to kill list: '{p.MainModule.FileName}' (ID: {p.Id})");
                }
            }

            foreach (Process p in newProcessesAcrobat) {
                if (p.MainModule?.FileName != null) {
                    processesToKill.Add(new AdobeMemoryWastingCrap(p.MainModule.FileName, p.Id));
                    Logger.Log($"PopulateGrid(): Added Acrobat process to kill list: '{p.MainModule.FileName}' (ID: {p.Id})");
                }
            }
#else
            foreach (Process p in newProcessesBoth) {
                if (p.MainModule?.FileName != null) {
                    processesToKill.Add(new AdobeMemoryWastingCrap(p.MainModule.FileName, p.Id));
                    Logger.Log($"PopulateGrid(): Added Acrobat process to kill list: '{p.MainModule.FileName}' (ID: {p.Id})");
                }
            }
#endif

            Logger.Log("PopulateGrid(): Process list grid population completed.");
        }

        /// <summary>
        /// Check if program is running as admin
        /// </summary>
        /// <returns>true, if running with elevated privileges</returns>
        private static bool IsAdministrator() {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Helper function to take a nullable string and return a fixed string
        /// </summary>
        /// <param name="input">input nullable string</param>
        /// <returns>the input string if not empty, else an empty string</returns>
        private static string SafeString(string? input) {
            if (!string.IsNullOrWhiteSpace(input)) return input;

            return "";
        }
#endregion
    }
}
