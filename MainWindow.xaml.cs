using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;

namespace AdobeCrapKiller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<AdobeMemoryWastingCrap> processesToKill { get; set; }
        private System.Windows.Threading.DispatcherTimer getProcessStatusTimer = new();

        public MainWindow()
        {
            InitializeComponent();

            // Restart program and run as admin
            if (!IsAdministrator())
            {
                if (System.Diagnostics.Process.GetCurrentProcess().MainModule is ProcessModule exeModule)
                {
                    string elevateTitle = "Restart program as Administrator?";
                    string elevateMessage = "Admin privileges are required to kill 100% of all Adobe crap. \n \nSelect yes to run this app as admin. \nSelect no to try running as-is.";
                    if (MessageBox.Show(elevateMessage, elevateTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        string exeName = SafeString(exeModule.FileName);
                        ProcessStartInfo startInfo = new(exeName)
                        {
                            Verb = "runas",
                            UseShellExecute = true
                        };
                        System.Diagnostics.Process.Start(startInfo);
                        Application.Current.Shutdown();
                        return;
                    }
                }
            } else
            {
                // Mark title as red if we are running as admin, to make it more clear
                lblTitle.Foreground = new SolidColorBrush(Colors.Red);
                lblTitle.Content += " (Admin Mode)";
            }

            // Show version in titlebar
            if (Assembly.GetExecutingAssembly().GetName().Version != null) {
                this.Title += " v";
                this.Title += Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

            // Setup auto-refresh timer properties
            getProcessStatusTimer.Tick += new EventHandler(getProcessStatusTimer_Tick);
            getProcessStatusTimer.Interval = new TimeSpan(0, 0, 0, 3);

            // Setup datagrid binding
            processesToKill = new ObservableCollection<AdobeMemoryWastingCrap>();
            dataGrid.ItemsSource = processesToKill;
        }

        #region Event handlers
        private void btnAutoRefresh_Click(object sender, RoutedEventArgs e)
        {
            // Toggle button states
            btnAutoRefresh.IsEnabled = !btnAutoRefresh.IsEnabled;
            btnStop.IsEnabled = !btnStop.IsEnabled;

            // Start auto-refresh timer to show process state
            getProcessStatusTimer.Start();

        }

        private void btnKill_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in processesToKill)
            {
                ProcessExtensions.KillByPath(item.ProcessPath);
            }

            PopulateGrid();
            SystemSounds.Beep.Play();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PopulateGrid();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            // Stop auto-refresh timer to show process state
            getProcessStatusTimer.Stop();

            // Toggle button states
            btnAutoRefresh.IsEnabled = !btnAutoRefresh.IsEnabled;
            btnStop.IsEnabled = !btnStop.IsEnabled;
        }

        private void getProcessStatusTimer_Tick(object? sender, EventArgs e)
        {
            //SystemSounds.Beep.Play();
            PopulateGrid();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Do the first refresh
            PopulateGrid();
        }
        #endregion

        #region Local methods
        private void PopulateGrid()
        {
            List<Process> newProcessesAdobe = ProcessExtensions.GetByPathSubstring("adobe");
            List<Process> newProcessesAcrobat = ProcessExtensions.GetByPathSubstring("acrobat");

            processesToKill.Clear();

            foreach (Process p in newProcessesAdobe)
            {
                processesToKill.Add(new AdobeMemoryWastingCrap(p.MainModule.FileName));
            }

            foreach (Process p in newProcessesAcrobat)
            {
                processesToKill.Add(new AdobeMemoryWastingCrap(p.MainModule.FileName));
            }
        }

        /// <summary>
        /// Check if program is running as admin
        /// </summary>
        /// <returns>true, if running with elevated privileges</returns>
        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Helper function to take a nullable string and return a fixed string
        /// </summary>
        /// <param name="input">input nullable string</param>
        /// <returns>the input string if not empty, else an empty string</returns>
        private static string SafeString(string? input)
        {
            if (!string.IsNullOrWhiteSpace(input)) return input;

            return "";
        }
        #endregion
    }

    public class AdobeMemoryWastingCrap
    {
        public string ProcessPath { get; set; }

        public AdobeMemoryWastingCrap(string processPath)
        {
            ProcessPath = processPath;
        }
    }
}
