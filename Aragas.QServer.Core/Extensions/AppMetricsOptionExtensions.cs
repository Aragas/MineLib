using App.Metrics;
using App.Metrics.Infrastructure;

using System;
using System.Reflection;
using System.Runtime.InteropServices;

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
        public static MetricsOptions AddGitTag(this MetricsOptions options)
        {
            options.GlobalTags["branch"] = ThisAssembly.Git.Branch;
            options.GlobalTags["sha"] = ThisAssembly.Git.Sha;
            options.GlobalTags["is_dirty"] = ThisAssembly.Git.IsDirtyString;

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