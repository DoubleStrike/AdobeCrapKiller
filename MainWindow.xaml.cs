using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            System.Windows.Threading.DispatcherTimer getProcessStatusTimer = new System.Windows.Threading.DispatcherTimer();
            getProcessStatusTimer.Tick += new EventHandler(getProcessStatusTimer_Tick);
            getProcessStatusTimer.Interval = new TimeSpan(0, 0, 0, 1);
            getProcessStatusTimer.Start();

        }

        private void getProcessStatusTimer_Tick(object sender, EventArgs e)
        {
            // HACK: This is simple brute force for now, as a concept test
            lblProcess01.FontWeight = ProcessExtensions.IsRunning(lblProcess01.Content.ToString()) ? FontWeights.Bold : FontWeights.Regular;
            lblProcess02.FontWeight = ProcessExtensions.IsRunning(lblProcess02.Content.ToString()) ? FontWeights.Bold : FontWeights.Regular;
        }
    }
}
