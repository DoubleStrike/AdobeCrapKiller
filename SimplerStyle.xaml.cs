using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;

namespace AdobeCrapKiller
{
    /// <summary>
    /// Interaction logic for SimplerStyle.xaml
    /// </summary>
    public partial class SimplerStyle : Window
    {
        public ObservableCollection<AdobeMemoryWastingCrap> processesToKill { get; set; }

        public SimplerStyle()
        {
            InitializeComponent();

            if (!IsAdministrator() && false)
            {
                // Restart program and run as admin
                if (!IsAdministrator())
                {
                    if (false && System.Diagnostics.Process.GetCurrentProcess().MainModule is ProcessModule exeModule)
                    {
                        string exeName = SafeString(exeModule.FileName);
                        ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                        startInfo.Verb = "runas";
                        System.Diagnostics.Process.Start(startInfo);
                        Application.Current.Shutdown();
                        return;
                    }
                } else
                {
                    lblTitle.Foreground = new SolidColorBrush(Colors.Red);
                }
            }

            processesToKill = new ObservableCollection<AdobeMemoryWastingCrap>();
        }

        private void btnAutoRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PopulateGrid();
            dataGrid.ItemsSource = processesToKill;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PopulateGrid()
        {
            List<Process> newProcessesAdobe = ProcessExtensions.GetByPathSubstring("adobe");
            List<Process> newProcessesAcrobat = ProcessExtensions.GetByPathSubstring("acrobat");

            foreach (Process p in newProcessesAdobe) {
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
            WindowsPrincipal principal = new WindowsPrincipal(identity);
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
