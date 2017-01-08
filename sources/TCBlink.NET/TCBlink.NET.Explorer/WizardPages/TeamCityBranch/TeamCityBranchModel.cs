using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using TCBlink.NET.Common;
using TCBlink.NET.Explorer.ViewModel;
using TeamCitySharp;
using TeamCitySharp.Locators;

namespace TCBlink.NET.Explorer.WizardPages.TeamCityBranch
{
    public class TeamCityBranchModel : ViewModelBase
    {
        #region constructors

        public TeamCityBranchModel(TCBlinkConfig blinkConfig)
        {
            _blinkConfig = blinkConfig;
            CheckBranch = new RelayCommand(async () => await CheckIfBranchExists(), () => !string.IsNullOrEmpty(Branch));
            MessengerInstance.Register(this, new Action<FinishMessage>(message =>
            {
                var saveDialog = new SaveFileDialog
                {
                    AddExtension = true,
                    CheckPathExists = true,
                    DefaultExt = ".json",
                    FileName = "config.json",
                    Title = "Save configuration"
                };

                if (!saveDialog.ShowDialog(Application.Current.MainWindow).Value) return;
                try
                {
                    File.WriteAllText(saveDialog.FileName, JsonConvert.SerializeObject(_blinkConfig));
                }
                catch (Exception)
                {
                    MessageBox.Show(Application.Current.MainWindow, "Failed to save configuration", "Error", MessageBoxButton.OK);
                }
            }));
        }

        #endregion

        #region commands

        public ICommand CheckBranch { get; }

        #endregion

        #region methods

        private async Task CheckIfBranchExists()
        {
            IsLoading = true;
            await Task.Run(() =>
            {
                try
                {
                    var tcClient = new TeamCityClient(_blinkConfig.TeamCityConfig.HostName, _blinkConfig.TeamCityConfig.UseSsl);
                    tcClient.Connect(_blinkConfig.TeamCityConfig.UserName, _blinkConfig.TeamCityConfig.Password);
                    if (!tcClient.Authenticate()) return;
                    var latestBuild = tcClient.Builds
                        .ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(_blinkConfig.TeamCityConfig.BuildConfigurationId), branch: Branch, maxResults: 1))
                        .SingleOrDefault();

                    IsValid = latestBuild != null;
                }
                catch (Exception)
                {
                    IsValid = false;
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }

        #endregion

        #region fields

        private readonly TCBlinkConfig _blinkConfig;
        private bool _isValid;
        private bool _isLoading;

        #endregion

        #region properties

        public string Branch
        {
            get { return _blinkConfig.TeamCityConfig.Branch; }
            set
            {
                _blinkConfig.TeamCityConfig.Branch = value;
                RaisePropertyChanged();
            }
        }

        public double PollingInterval
        {
            get { return _blinkConfig.TeamCityConfig.PollingInterval; }
            set
            {
                _blinkConfig.TeamCityConfig.PollingInterval = value;
                RaisePropertyChanged();
            }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}