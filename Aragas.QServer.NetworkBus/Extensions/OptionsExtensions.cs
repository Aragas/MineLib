using System;

namespace NATS.Client
{
    public static class OptionsExtensions
    {
        private static bool InContainer => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is string str && str == "true";

        public static Options SetDefaultArgs(this Options options, TimeSpan? messageTTL = null)
        {
            options.Timeout = (int) messageTTL.GetValueOrDefault(TimeSpan.FromMilliseconds(10000)).TotalMilliseconds;
            options.AllowReconnect = true;
            options.MaxReconnect = Options.ReconnectForever;
            options.Url = InContainer ? "nats:4222" : "localhost:4222";
            return options;
        }
    }
}