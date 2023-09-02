using System;
using System.Diagnostics;
using System.Media;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdobeCrapKiller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Restart program and run as admin
            if (!IsAdministrator())
            {
                if (System.Diagnostics.Process.GetCurrentProcess().MainModule is ProcessModule exeModule)
                {
                    string exeName = SafeString(exeModule.FileName);
                    ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                    startInfo.Verb = "runas";
                    System.Diagnostics.Process.Start(startInfo);
                    Application.Current.Shutdown();
                    return;
                }
            }


            // Start 1 second refresh timer to show process state
            System.Windows.Threading.DispatcherTimer getProcessStatusTimer = new System.Windows.Threading.DispatcherTimer();
            getProcessStatusTimer.Tick += new EventHandler(getProcessStatusTimer_Tick);
            getProcessStatusTimer.Interval = new TimeSpan(0, 0, 0, 1);
            getProcessStatusTimer.Start();

        }

        private void btnKill_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();

            if (lblProcess01.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess01.Content.ToString()));
            if (lblProcess02.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess02.Content.ToString()));
            if (lblProcess03.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess03.Content.ToString()));
            if (lblProcess04.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess04.Content.ToString()));
            if (lblProcess05.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess05.Content.ToString()));
            if (lblProcess06.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess06.Content.ToString()));
            if (lblProcess07.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess07.Content.ToString()));
            if (lblProcess08.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess08.Content.ToString()));
            if (lblProcess09.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess09.Content.ToString()));
            if (lblProcess10.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess10.Content.ToString()));
            if (lblProcess11.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess11.Content.ToString()));
            if (lblProcess12.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess12.Content.ToString()));
            if (lblProcess13.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess13.Content.ToString()));
            if (lblProcess14.FontWeight == FontWeights.Bold) ProcessExtensions.KillByName(SafeString(lblProcess14.Content.ToString()));
        }

        private void getProcessStatusTimer_Tick(object? sender, EventArgs e)
        {
            // HACK: This is simple brute force for now, as a concept test
            lblProcess01.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess01.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess02.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess02.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess03.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess03.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess04.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess04.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess05.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess05.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess06.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess06.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess07.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess07.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess08.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess08.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess09.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess09.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess10.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess10.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess11.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess11.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess12.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess12.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess13.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess13.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess14.FontWeight = ProcessExtensions.IsRunning(SafeString(lblProcess14.Content.ToString())) ? FontWeights.Bold : FontWeights.Regular;
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

    public class AdobeProcess
    {
        private string s_processName = "";

        public string? ProcessName
        {
            get { return this.s_processName; }

            set { s_processName = string.IsNullOrEmpty(value) ? "" : value; }
        }
    }
}
