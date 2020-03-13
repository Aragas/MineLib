using Aragas.QServer.Hosting.Modules;

using Volo.Abp.Modularity;

namespace Aragas.QServer.Hosting
{
    [DependsOn(typeof(ServiceModule))]
    [DependsOn(typeof(NATSModule))]
    [DependsOn(typeof(NATSSubscriptionStorageModule))]
    [DependsOn(typeof(MetricsModule))]
    [DependsOn(typeof(HealthModule))]
    [DependsOn(typeof(MetricsReporterNATSSubscriptionStorageModule))]
    [DependsOn(typeof(HealthReporterNATSSubscriptionStorageModule))]
    public class QServerModule : AbpModule { }
}