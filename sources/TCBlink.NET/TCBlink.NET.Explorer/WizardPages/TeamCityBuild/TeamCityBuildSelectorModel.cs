using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using TCBlink.NET.Common;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace TCBlink.NET.Explorer.WizardPages.TeamCityBuild
{
    public class TeamCityBuildSelectorModel : ViewModelBase
    {

        #region private fields

        private readonly TCBlinkConfig _blinkConfig;
        private ObservableCollection<BuildConfig> _availableBuildConfigurations;

        #endregion

        #region constructors

        public TeamCityBuildSelectorModel(TCBlinkConfig blinkConfig)
        {
            _blinkConfig = blinkConfig;
            UpdateAvailableBuildConfigurations = new RelayCommand(async () => await GetAvailableBuildConfigurations());
        }

        #endregion

        #region properties

        public ObservableCollection<BuildConfig> AvailableBuildConfigurations
        {
            get { return _availableBuildConfigurations; }
            set
            {
                _availableBuildConfigurations = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region command properties

        public ICommand UpdateAvailableBuildConfigurations { get; }

        #endregion

        #region methods

        private async Task GetAvailableBuildConfigurations()
        {
            await Task.Run(() =>
            {
                var tcClient = new TeamCityClient(_blinkConfig.TeamCityConfig.HostName, _blinkConfig.TeamCityConfig.UseSsl);
                tcClient.Connect(_blinkConfig.TeamCityConfig.UserName, _blinkConfig.TeamCityConfig.Password);
                if (tcClient.Authenticate())
                {
                    AvailableBuildConfigurations = new ObservableCollection<BuildConfig>(tcClient.BuildConfigs.All());
                }
            });
        }

        #endregion
    }
}