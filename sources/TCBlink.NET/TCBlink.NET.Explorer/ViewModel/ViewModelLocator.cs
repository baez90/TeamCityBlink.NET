using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using TCBlink.NET.Common;
using TCBlink.NET.Explorer.WizardPages.ColorSelector;
using TCBlink.NET.Explorer.WizardPages.TeamCityBranch;
using TCBlink.NET.Explorer.WizardPages.TeamCityBuild;
using TCBlink.NET.Explorer.WizardPages.TeamCityConnection;

namespace TCBlink.NET.Explorer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {

        private static readonly TCBlinkConfig _blinkConfig = new TCBlinkConfig();

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register(() => new MainViewModel());
            SimpleIoc.Default.Register(() => new ColorSelectorModel(_blinkConfig));
            SimpleIoc.Default.Register(() => new TeamCityConnectionModel(_blinkConfig));
            SimpleIoc.Default.Register(() => new TeamCityBuildSelectorModel(_blinkConfig));
            SimpleIoc.Default.Register(() => new TeamCityBranchModel(_blinkConfig));
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ColorSelectorModel ColorSelector => ServiceLocator.Current.GetInstance<ColorSelectorModel>();

        public TeamCityConnectionModel TeamCityConnection => ServiceLocator.Current.GetInstance<TeamCityConnectionModel>();

        public TeamCityBuildSelectorModel TeamCityBuildSelector => ServiceLocator.Current.GetInstance<TeamCityBuildSelectorModel>();

        public TeamCityBranchModel TeamCityBranch => ServiceLocator.Current.GetInstance<TeamCityBranchModel>();

        public static TCBlinkConfig BlinkConfig => _blinkConfig;

        public static void Cleanup()
        {
            SimpleIoc.Default.GetAllCreatedInstances<ColorSelectorModel>().ToList().ForEach(m => m.Dispose());
        }
    }
}