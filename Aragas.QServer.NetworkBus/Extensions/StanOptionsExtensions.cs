using System;

namespace STAN.Client
{
    public static class StanOptionsExtensions
    {
        private static bool InContainer => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is string str && str == "true";

        public static StanOptions SetDefaultArgs(this StanOptions options, TimeSpan? messageTTL = null)
        {
            options.ConnectTimeout = (int) messageTTL.GetValueOrDefault(TimeSpan.FromMilliseconds(10000)).TotalMilliseconds;
            options.NatsURL = InContainer ? "aragas.nats:4222" : "localhost:4222";
            return options;
        }
    }
}