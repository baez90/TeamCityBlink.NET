using System;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using TCBlink.NET.Explorer.Models;
using ThingM.Blink1;

namespace TCBlink.NET.Explorer.WizardPages.ColorSelector
{
    public class ColorSelectorModel : ViewModelBase, IDisposable
    {
        private readonly Blink1 _blink;
        private readonly BlinkConfig _blinkConfig;

        public ColorSelectorModel(BlinkConfig blinkConfig)
        {
            _blinkConfig = blinkConfig;

            _blink = new Blink1();
            _blink.Open();

            UpdateSelectedColor = new RelayCommand<Color>(UpdateBlinkColor);
        }

        #region properties

        public Color SuccessColor
        {
            get { return _blinkConfig.SuccessColor; }
            set
            {
                _blinkConfig.SuccessColor = value;
                RaisePropertyChanged();
            }
        }

        public Color ErrorColor
        {
            get { return _blinkConfig.ErrorColor; }
            set
            {
                _blinkConfig.ErrorColor = value;
                RaisePropertyChanged();
            }
        }

        #endregion



        #region command properties

        public ICommand UpdateSelectedColor { get; }

        #endregion

        #region methods

        private void UpdateBlinkColor(Color newColor)
        {
            if (_blink.IsConnected)
            {
                _blink.SetColor(newColor.R, newColor.G, newColor.B);
            }
        }

        public void Dispose()
        {
            if (_blink.IsConnected)
            {
                _blink.Close();
            }
        }

        #endregion
    }
}