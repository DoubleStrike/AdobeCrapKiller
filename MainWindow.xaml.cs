﻿using System;
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

        private void btnKill_Click(object sender, RoutedEventArgs e)
        {
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

        private void getProcessStatusTimer_Tick(object sender, EventArgs e)
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

        private static string SafeString(string? input)
        {
            if (!string.IsNullOrWhiteSpace(input)) return input;

            return "";
        }
    }
}
