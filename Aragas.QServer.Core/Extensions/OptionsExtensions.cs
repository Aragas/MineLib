using NATS.Client;

using STAN.Client;

using System;

namespace Aragas.QServer.Core.Extensions
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
        public static StanOptions SetDefaultArgs(this StanOptions options, TimeSpan? messageTTL = null)
        {
            options.ConnectTimeout = (int) messageTTL.GetValueOrDefault(TimeSpan.FromMilliseconds(10000)).TotalMilliseconds;
            options.NatsURL = InContainer ? "aragas.nats:4222" : "localhost:4222";
            return options;
        }
    }
}