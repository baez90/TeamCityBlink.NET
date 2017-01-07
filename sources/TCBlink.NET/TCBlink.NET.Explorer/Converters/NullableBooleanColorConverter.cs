using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TCBlink.NET.Explorer.Converters
{
    public class NullableBooleanColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = value as bool?;
            if (!booleanValue.HasValue) return new SolidColorBrush(Color.FromScRgb(0, 255, 255, 255));
            return new SolidColorBrush(booleanValue.Value ? Color.FromRgb(0, 255, 0) : Color.FromRgb(255, 0, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}