using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;

namespace AdobeCrapKiller {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        // Change the property to be nullable to satisfy CS8618
        System.Collections.ObjectModel.ObservableCollection<AdobeMemoryWastingCrap>? processesToKill { get; set; }
        private System.Windows.Threading.DispatcherTimer getProcessStatusTimer = new();

        public MainWindow() {
            Logger.Log("MainWindow(): Initializing main window.", LogLevel.Info);
            InitializeComponent();

            // Restart program and run as admin
            if (!IsAdministrator()) {
                Logger.Log("MainWindow(): Not running as admin, requesting elevation.", LogLevel.Info);
                if (System.Diagnostics.Process.GetCurrentProcess().MainModule is ProcessModule exeModule) {
                    string elevateTitle = "Restart program as Administrator?";
                    string elevateMessage = "Admin privileges are required to kill 100% of all Adobe crap. \n \nSelect yes to run this app as admin. \nSelect no to try running as-is.";
                    if (MessageBox.Show(elevateMessage, elevateTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                        string exeName = SafeString(exeModule.FileName);
                        ProcessStartInfo startInfo = new(exeName) {
                            Verb = "runas",
                            UseShellExecute = true
                        };
                        Logger.Log("MainWindow(): Restarting program as admin: " + exeName, LogLevel.Info);
                        System.Diagnostics.Process.Start(startInfo);

                        Logger.Log("MainWindow(): Exiting current non-admin instance.", LogLevel.Info);
                        Application.Current.Shutdown();
                        return;
                    } else {
                        Logger.Log("MainWindow(): User chose to continue without admin privileges.", LogLevel.Info);
                    }
                }
            } else {
                Logger.Log("MainWindow(): Running as admin.", LogLevel.Info);

                // Mark title as red if we are running as admin, to make it more clear
                lblTitle.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                lblTitle.Content += " (Admin Mode)";
            }

            // Show version in titlebar
            Version? version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null) {
                this.Title += " v";
                this.Title += version.ToString();
            }

            // Setup auto-refresh timer properties
            Logger.Log("MainWindow(): Setting up auto-refresh timer.", LogLevel.Info);
            getProcessStatusTimer.Tick += new EventHandler(getProcessStatusTimer_Tick);
            getProcessStatusTimer.Interval = new TimeSpan(0, 0, 0, 10);

            // Setup datagrid binding
            Logger.Log("MainWindow(): Setting up datagrid binding.", LogLevel.Info);
            processesToKill = new System.Collections.ObjectModel.ObservableCollection<AdobeMemoryWastingCrap>();
            dataGrid.ItemsSource = processesToKill;

            Logger.Log("MainWindow(): Main window initialization completed.", LogLevel.Info);
        }

        #region Event handlers
        private void btnAutoRefresh_Click(object sender, RoutedEventArgs e) {
            Logger.Log("btnAutoRefresh_Click(): Auto-refresh started.", LogLevel.Info);

            // Toggle button states
            btnAutoRefresh.IsEnabled = !btnAutoRefresh.IsEnabled;
            btnStop.IsEnabled = !btnStop.IsEnabled;

            // Start auto-refresh timer to show process state
            getProcessStatusTimer.Start();
        }

        private void btnKill_Click(object sender, RoutedEventArgs e) {
            Logger.Log("btnKill_Click(): Killing Adobe processes.", LogLevel.Info);

            if (processesToKill == null) return;

            foreach (var item in processesToKill) {
                Logger.Log($"btnKill_Click(): Killing process at path: '{item.ProcessPath}' (ID: {item.ProcessId})", LogLevel.Info);
                ProcessExtensions.KillById(item.ProcessId);
            }

            PopulateGrid();
            System.Media.SystemSounds.Beep.Play();
            Logger.Log("btnKill_Click(): Finished killing Adobe processes.", LogLevel.Info);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            Logger.Log("btnRefresh_Click(): Manual refresh triggered.", LogLevel.Info);
            PopulateGrid();
            Logger.Log("btnRefresh_Click(): Manual refresh completed.", LogLevel.Info);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e) {
            Logger.Log("btnStop_Click(): Auto-refresh stopped.", LogLevel.Info);

            // Stop auto-refresh timer to show process state
            getProcessStatusTimer.Stop();

            // Toggle button states
            btnAutoRefresh.IsEnabled = !btnAutoRefresh.IsEnabled;
            btnStop.IsEnabled = !btnStop.IsEnabled;
        }

        private void getProcessStatusTimer_Tick(object? sender, EventArgs e) {
            Logger.Log("getProcessStatusTimer_Tick(): Auto-refresh tick triggered.", LogLevel.Info);
            System.Media.SystemSounds.Beep.Play();
            PopulateGrid();
            Logger.Log("getProcessStatusTimer_Tick(): Auto-refresh tick completed.", LogLevel.Info);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            Logger.Log("Window_Loaded(): Window loaded event triggered.", LogLevel.Info);

            // Do the first refresh
            PopulateGrid();

            Logger.Log("Window_Loaded(): Initial process list population completed.", LogLevel.Info);
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
            Logger.Log("PopulateGrid(): Populating process list grid.", LogLevel.Info);
            // New code for testing
            System.Collections.Generic.List<Process> newProcessesBoth = ProcessExtensions.GetByPathSubstrings(new[] { "adobe", "acrobat" });
            Logger.Log("PopulateGrid(): Found " + newProcessesBoth.Count + " Adobe/Acrobat-related processes.", LogLevel.Info);

            processesToKill ??= new();

            processesToKill.Clear();

            foreach (Process p in newProcessesBoth) {
                if (p.MainModule?.FileName != null) {
                    processesToKill.Add(new AdobeMemoryWastingCrap(p.MainModule.FileName, p.Id));
                    Logger.Log($"PopulateGrid(): Added Acrobat process to kill list: '{p.MainModule.FileName}' (ID: {p.Id})", LogLevel.Info);
                }
            }

            Logger.Log("PopulateGrid(): Process list grid population completed.", LogLevel.Info);
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
