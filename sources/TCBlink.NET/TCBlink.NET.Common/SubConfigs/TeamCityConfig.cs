﻿namespace TCBlink.NET.Common.SubConfigs
{
    public class TeamCityConfig
    {
        public double PollingInterval { get; set; } = 10000d;

        public string HostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool UseSsl { get; set; } = true;

        public string BuildConfigurationId { get; set; }

        public string Branch { get; set; }
    }
}