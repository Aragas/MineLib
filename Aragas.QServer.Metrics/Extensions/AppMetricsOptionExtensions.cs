using App.Metrics.Infrastructure;

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace App.Metrics
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
        public static MetricsOptions AddGitTag(this MetricsOptions options)
        {
            var metadatas = Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyMetadataAttribute>();
            options.GlobalTags["branch"] = metadatas.SingleOrDefault(a => a.Key == "GitInfo.Branch")?.Value ?? "unknown";
            options.GlobalTags["sha"] = metadatas.SingleOrDefault(a => a.Key == "GitInfo.Sha")?.Value ?? "unknown";
            options.GlobalTags["is_dirty"] = metadatas.SingleOrDefault(a => a.Key == "GitInfo.IsDirty")?.Value ?? "unknown";

            return options;
        }
        public static MetricsOptions AddRuntimeTag(this MetricsOptions options)
        {
            options.GlobalTags["dotnet_runtime"] = RuntimeInformation.FrameworkDescription;
            options.GlobalTags["version"] = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion; 

            return options;
        }
    }
}