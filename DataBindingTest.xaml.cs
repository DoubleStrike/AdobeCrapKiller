using System.Collections.Generic;
using System.Windows;

namespace AdobeCrapKiller
{
    /// <summary>
    /// Interaction logic for DataBindingTest.xaml
    /// </summary>
    public partial class DataBindingTest : Window
    {
        public DataBindingTest()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new TestingViewModel();
        }
    }

    public class SingleProcess
    {
        public string ProcessName { get; set; }
        public bool Active { get; set; }

        public SingleProcess(string processName)
        {
            ProcessName = processName;
            Active = false;
        }

        public SingleProcess(string processName, bool active) : this(processName)
        {
            Active = active;
        }
    }

    public class TestingViewModel
    {
        public List<SingleProcess> ProcessesToBind
        {
            get
            {
                return new List<SingleProcess>()
                {
                    new("x", true),
                    new("a"),
                    new("b"),
                    new("c"),
                    new("d"),
                    new("e"),
                };
            }
        }
    }
}
