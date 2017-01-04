using System.Windows.Media;

namespace TCBlink.NET.Explorer.Models
{
    public class BlinkConfig
    {
        public Color SuccessColor { get; set; } = Color.FromRgb(0, 255, 0);
        public Color ErrorColor { get; set; } = Color.FromRgb(255, 0, 0);
    }
}