using System;
using System.Linq;
using System.Timers;
using NLog;
using TCBlink.NET.Common;
using TCBlink.NET.Common.Exceptions;
using TCBlink.NET.Common.Extensions;
using TeamCitySharp;
using TeamCitySharp.Locators;
using ThingM.Blink1;
using ThingM.Blink1.ColorProcessor;

namespace TCBlink.NET.Service
{
    public class BlinkService : IDisposable
    {
        private readonly Blink1 _blink;
        private readonly TCBlinkConfig _config;
        private readonly Logger _logger;
        private readonly Timer _pollingTimer;
        private readonly Action<IColorProcessor> _setColor;
        private readonly TeamCityClient _teamCityClient;

        public BlinkService(TCBlinkConfig config)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _config = config;
            _logger.Debug("Initializing Timer");
            _pollingTimer = new Timer(_config.TeamCityConfig.PollingInterval)
            {
                Enabled = true,
                AutoReset = true
            };

            _blink = new Blink1();

            _logger.Debug("Initialize TeamCity connection");
            _teamCityClient = new TeamCityClient(_config.TeamCityConfig.HostName, _config.TeamCityConfig.UseSsl);
            _setColor = color =>
            {
                _logger.Debug("Updating Blink(1) indicator");
                if (_blink.IsConnected)
                    _blink.SetColor(color);
            };
        }

        public void Dispose()
        {
            _logger.Debug("Shutdown service. Releasing connection to Blink(1) device and terminate timer.");
            _blink?.Close();
            _blink?.Dispose();
            _pollingTimer?.Dispose();
        }

        public void OnStarting()
        {
            _logger.Debug("Connecting to TeamCity");
            try
            {
                _teamCityClient.Connect(_config.TeamCityConfig.UserName, _config.TeamCityConfig.Password);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to connect to TeamCity instance");
                throw;
            }

            try
            {
                _blink.Open();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to connect to Blink(1) device");
                throw;
            }

            if (!_teamCityClient.Authenticate())
            {
                _logger.Error("Failed to authenticate to TeamCity");
                throw new TCConectionFailException("Error while authenticating to teamcity");
            }

            _logger.Debug("Starting timer");
            _pollingTimer.Elapsed += (sender, args) => OnUpdate();
            _pollingTimer.Start();
        }

        public void OnStopping()
        {
            _logger.Debug("Stopping timer");
            _pollingTimer.Stop();
        }

        private void OnUpdate()
        {
            var tcConfig = _config.TeamCityConfig;
            var colorConfig = _config.ColorConfig;

            _logger.Debug("Getting latest build status");
            var latestBuild =
                _teamCityClient
                    .Builds
                    .ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(tcConfig.BuildConfigurationId), branch: tcConfig.Branch, maxResults: 1))
                    .SingleOrDefault();

            if (latestBuild != null)
                switch (latestBuild.Status)
                {
                    case "SUCCESS":
                        _setColor(colorConfig.SuccessColor.ToRgb());
                        break;
                    default:
                        _setColor(colorConfig.ErrorColor.ToRgb());
                        break;
                }
        }
    }
}