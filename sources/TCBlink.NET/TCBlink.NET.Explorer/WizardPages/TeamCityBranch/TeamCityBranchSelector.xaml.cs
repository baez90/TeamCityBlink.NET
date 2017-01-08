using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace TCBlink.NET.Explorer.WizardPages.TeamCityBranch
{
    /// <summary>
    ///     Interaction logic for TeamCityBranchSelector.xaml
    /// </summary>
    public partial class TeamCityBranchSelector : WizardPage
    {

        public static readonly DependencyProperty FinishCommandProperty = DependencyProperty.Register("FinishCommand", typeof(ICommand), typeof(TeamCityBranchSelector), new PropertyMetadata(null));

        public ICommand FinishCommand
        {
            get { return (ICommand) GetValue(FinishCommandProperty); }
            set { SetValue(FinishCommandProperty, value); }
        }

        public TeamCityBranchSelector()
        {
            InitializeComponent();
        }
    }
}