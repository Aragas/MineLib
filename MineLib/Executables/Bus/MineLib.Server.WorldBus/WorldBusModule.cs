using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace MineLib.Server.WorldBus
{
    [DependsOn(typeof(QServerModule))]
    public class WorldBusModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.Configure<ServiceOptions>(o => o.Name = "WorldBus");

            services.AddNpgSqlMetrics("Database", configuration["PostgreSQLConnectionString"]);

            services.AddSingleton<IWorldHandler, StandardWorldHandler>();

            services.AddSingleton<WorldHandlerManager>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var services = context.ServiceProvider;

            var serviceOptions = services.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = services.GetRequiredService<SubscriptionStorage>();

            subscriptionStorage.HandleChunksInSquare<WorldHandlerManager>();
            subscriptionStorage.HandleChunksInCircle<WorldHandlerManager>();
        }
    }
}