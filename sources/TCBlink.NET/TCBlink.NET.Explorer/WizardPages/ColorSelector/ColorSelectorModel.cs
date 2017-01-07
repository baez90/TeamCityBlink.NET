using System;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using TCBlink.NET.Common;
using ThingM.Blink1;

namespace TCBlink.NET.Explorer.WizardPages.ColorSelector
{
    public class ColorSelectorModel : ViewModelBase, IDisposable
    {

        #region fields

        private readonly Blink1 _blink;
        private readonly TCBlinkConfig _blinkConfig;

        #endregion

        #region constructors

        public ColorSelectorModel(TCBlinkConfig blinkConfig)
        {
            _blinkConfig = blinkConfig;

            _blink = new Blink1();
            _blink.Open();

            UpdateSelectedColor = new RelayCommand<Color>(UpdateBlinkColor);
        }

        #endregion

        #region properties

        public Color SuccessColor
        {
            get { return _blinkConfig.ColorConfig.SuccessColor; }
            set
            {
                _blinkConfig.ColorConfig.SuccessColor = value;
                RaisePropertyChanged();
            }
        }

        public Color ErrorColor
        {
            get { return _blinkConfig.ColorConfig.ErrorColor; }
            set
            {
                _blinkConfig.ColorConfig.ErrorColor = value;
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