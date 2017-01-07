using TCBlink.NET.Common.SubConfigs;

namespace TCBlink.NET.Common
{
    public class TCBlinkConfig
    {

        public Blink1ColorConfig ColorConfig { get; set; } = new Blink1ColorConfig();

        public TeamCityConfig TeamCityConfig { get; set; } = new TeamCityConfig();

    }
}