using System.Windows.Media;
using ThingM.Blink1.ColorProcessor;

namespace TCBlink.NET.Common.Extensions
{
    public static class ColorExtensions
    {
        public static Rgb ToRgb(this Color color)
        {
            return new Rgb(color.R, color.G, color.B);
        }
    }
}