using System.Windows;
using TCBlink.NET.Explorer.ViewModel;

namespace TCBlink.NET.Explorer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (sender, args) => ViewModelLocator.Cleanup();
        }
    }
}