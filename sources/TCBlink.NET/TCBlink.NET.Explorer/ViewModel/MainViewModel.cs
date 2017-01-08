using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using TCBlink.NET.Explorer.WizardPages.TeamCityBranch;

namespace TCBlink.NET.Explorer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            Finish = new RelayCommand(() =>
            {
                MessengerInstance.Send(new FinishMessage());
            });
        }

        public ICommand Finish { get; }
    }

    public class FinishMessage
    {
        public Guid MessageId { get; } = Guid.NewGuid();
    }
}