using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using TCBlink.NET.Common;
using TeamCitySharp;

namespace TCBlink.NET.Explorer.WizardPages.TeamCityConnection
{
    public class TeamCityConnectionModel : ViewModelBase
    {
        #region constructors

        public TeamCityConnectionModel(TCBlinkConfig blinkConfig)
        {
            _blinkConfig = blinkConfig;
            TestTeamCityConnection = new RelayCommand<PasswordBox>(async pwdBox => await ValidateTeamCityConnection(pwdBox));
        }

        #endregion

        #region command properties

        public ICommand TestTeamCityConnection { get; }

        #endregion

        #region methods

        private async Task ValidateTeamCityConnection(PasswordBox pwdBox)
        {
            IsTesting = true;
            var password = pwdBox.Password;
            await Task.Run(async () =>
            {
                var tcClient = new TeamCityClient(Host, UseSsl);
                tcClient.Connect(UserName, password);
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    try
                    {
                        IsValid = tcClient.Authenticate();
                    }
                    catch (Exception e)
                    {
                        IsValid = false;
                    }
                    IsTesting = false;
                    _blinkConfig.TeamCityConfig.Password = password;
                });
            });
        }

        #endregion

        #region fields

        private readonly TCBlinkConfig _blinkConfig;
        private bool _isValid;
        private bool _isTesting;

        #endregion

        #region properties

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTesting
        {
            get { return _isTesting; }
            set
            {
                _isTesting = value;
                RaisePropertyChanged();
            }
        }

        public string Host
        {
            get { return _blinkConfig.TeamCityConfig.HostName; }
            set
            {
                _blinkConfig.TeamCityConfig.HostName = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get { return _blinkConfig.TeamCityConfig.UserName; }
            set
            {
                _blinkConfig.TeamCityConfig.UserName = value;
                RaisePropertyChanged();
            }
        }

        public bool UseSsl
        {
            get { return _blinkConfig.TeamCityConfig.UseSsl; }
            set
            {
                _blinkConfig.TeamCityConfig.UseSsl = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}