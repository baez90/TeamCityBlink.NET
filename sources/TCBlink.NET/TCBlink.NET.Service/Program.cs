using System;
using System.IO;
using Newtonsoft.Json;
using NLog;
using TCBlink.NET.Common;
using Topshelf;

namespace TCBlink.NET.Service
{
    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "config.json");

        private static TCBlinkConfig _blinkConfig;

        public static void Main(string[] args)
        {
            Logger.Debug("Starting service");

            try
            {
                _blinkConfig = JsonConvert.DeserializeObject<TCBlinkConfig>(File.ReadAllText(ConfigFilePath));
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Failed to read configuration from path {ConfigFilePath}");
            }
            if (_blinkConfig == null) return;
            HostFactory.Run(configurator =>
            {
                configurator.Service<BlinkService>(service =>
                {
                    service.ConstructUsing(() => new BlinkService(_blinkConfig));
                    service.WhenStarted(s => s.OnStarting());
                    service.WhenStopped(s => s.OnStopping());
                    service.WhenShutdown(s => s.Dispose());
                });

                configurator.StartAutomaticallyDelayed();
                configurator.EnableShutdown();
                configurator.RunAsLocalSystem();
                configurator.SetServiceName("TCBlinkService");
                configurator.SetDisplayName("Teamcity Blink(1) Indicator");
                configurator.SetDescription("TeamCity Blink(1) Indicator");
                configurator.DependsOnEventLog();
                configurator.OnException(exception => { Logger.Error(exception); });
            });
        }
    }
}