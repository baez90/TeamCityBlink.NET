using System;
using System.IO;
using System.Windows.Media;
using Newtonsoft.Json;
using NUnit.Framework;
using TCBlink.NET.Common.SubConfigs;

namespace TCBlink.NET.Common.Tests
{
    [TestFixture]
    public class TCBlinkConfigSerializationTests
    {
        [Test]
        public void TestSerializeConfig()
        {
            var targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.json");
            var blinkConfig = new TCBlinkConfig
            {
                TeamCityConfig = new TeamCityConfig
                {
                    HostName = "teamcity.jetbrains.com",
                    UseSsl = true,
                    PollingInterval = 2000
                },
                ColorConfig = new Blink1ColorConfig
                {
                    SuccessColor = Color.FromRgb(0, 255, 0),
                    ErrorColor = Color.FromRgb(255, 0, 0)
                }
            };

            File.WriteAllText(targetPath, JsonConvert.SerializeObject(blinkConfig));
        }
    }
}