using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace Aragas.QServer.Hosting.Modules
{
    [DependsOn(typeof(ServiceModule))]
    [DependsOn(typeof(MetricsModule))]
    [DependsOn(typeof(NATSSubscriptionStorageModule))]
    public class MetricsReporterNATSSubscriptionStorageModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            var serviceProvider = context.ServiceProvider;

            var serviceOptions = serviceProvider.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = serviceProvider.GetRequiredService<SubscriptionStorage>();

            subscriptionStorage.HandleMetricsPrometheusHandler(serviceOptions.Uid);
        }
    }
}