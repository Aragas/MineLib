using System;

using NATS.Client;
using STAN.Client;

namespace Aragas.QServer.Core.Extensions
{
    public static class OptionsExtensions
    {
        private static bool InContainer => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        public static Options SetDefaultArgs(this Options options, TimeSpan? messageTTL = null)
        {
            options.Timeout = (int) messageTTL.GetValueOrDefault(TimeSpan.FromMilliseconds(10000)).TotalMilliseconds;
            options.AllowReconnect = true;
            options.MaxReconnect = Options.ReconnectForever;
            if (InContainer)
                options.Url = "aragas.nats:4222";
            else
                options.Url = "localhost:4222";
            return options;
        }
        public static StanOptions SetDefaultArgs(this StanOptions options, TimeSpan? messageTTL = null)
        {
            options.ConnectTimeout = (int) messageTTL.GetValueOrDefault(TimeSpan.FromMilliseconds(10000)).TotalMilliseconds;
            if (InContainer)
                options.NatsURL = "aragas.nats:4222";
            else
                options.NatsURL = "localhost:4222";
            return options;
        }
    }
}