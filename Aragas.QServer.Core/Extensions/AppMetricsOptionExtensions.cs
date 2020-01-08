using App.Metrics;
using App.Metrics.Infrastructure;

using System;

namespace Aragas.QServer.Core.Extensions
{
    public static class AppMetricsOptionExtensions
    {
        private static readonly EnvironmentInfo EnvInfo = new EnvironmentInfoProvider().Build();

        public static MetricsOptions AddMachineNameTag(this MetricsOptions options, string? machineName = null)
        {
            options.GlobalTags["machine"] = machineName ?? EnvInfo.MachineName;

            return options;
        }
        public static MetricsOptions AddUUIDTag(this MetricsOptions options, Guid uuid)
        {
            options.GlobalTags["uuid"] = uuid.ToString();

            return options;
        }
    }
}