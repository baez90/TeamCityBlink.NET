using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using TCBlink.NET.Explorer.Models;
using TCBlink.NET.Explorer.WizardPages.ColorSelector;

namespace TCBlink.NET.Explorer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {

        private static readonly BlinkConfig _blinkConfig = new BlinkConfig();

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register(() => new MainViewModel());
            SimpleIoc.Default.Register(() => new ColorSelectorModel(_blinkConfig));
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ColorSelectorModel ColorSelector => ServiceLocator.Current.GetInstance<ColorSelectorModel>();

        public static BlinkConfig BlinkConfig => _blinkConfig;

        public static void Cleanup()
        {
            SimpleIoc.Default.GetAllCreatedInstances<ColorSelectorModel>().ToList().ForEach(m => m.Dispose());
        }
    }
}