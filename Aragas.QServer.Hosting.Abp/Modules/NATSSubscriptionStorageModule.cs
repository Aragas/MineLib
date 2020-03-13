using Aragas.QServer.NetworkBus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace Aragas.QServer.Hosting.Modules
{
    [DependsOn(typeof(ServiceModule))]
    [DependsOn(typeof(NATSModule))]
    public class NATSSubscriptionStorageModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.AddSingleton<SubscriptionStorage>();
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            var serviceProvider = context.ServiceProvider;

            var subscriptionStorage = serviceProvider.GetRequiredService<SubscriptionStorage>();
            subscriptionStorage.HandleServiceDiscoveryHandler();

            var lifeTime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();
            lifeTime.ApplicationStopping.Register(() => subscriptionStorage.Dispose());
        }
    }
}