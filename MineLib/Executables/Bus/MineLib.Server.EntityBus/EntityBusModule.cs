using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace MineLib.Server.EntityBus
{
    [DependsOn(typeof(QServerModule))]
    public class EntityBusModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.Configure<ServiceOptions>(o => o.Name = "EntityBus");

            services.AddNpgSqlMetrics("Database", configuration["PostgreSQLConnectionString"]);

            services.AddSingleton<EntityHandlerManager>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var services = context.ServiceProvider;

            var subscriptionStorage = services.GetRequiredService<SubscriptionStorage>();
            subscriptionStorage.HandleGetNewEntityId<EntityHandlerManager>();
        }
    }
}