using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace MineLib.Server.PlayerBus
{
    [DependsOn(typeof(QServerModule))]
    public class PlayerBusModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.Configure<ServiceOptions>(o => o.Name = "PlayerBus");

            services.AddNpgSqlMetrics("Database", configuration["PostgreSQLConnectionString"]);

            services.AddSingleton<PlayerHandlerManager>();
            services.AddSingleton<PlayerTest>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var services = context.ServiceProvider;

            var serviceOptions = services.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = services.GetRequiredService<SubscriptionStorage>();

            subscriptionStorage.HandleGetExistingPlayerHandler<PlayerHandlerManager>();
            subscriptionStorage.HandleGetNewPlayerHandler<PlayerHandlerManager>(serviceOptions.Uid);

            subscriptionStorage.HandlePlayerPosition<PlayerTest>();
            subscriptionStorage.HandlePlayerLook<PlayerTest>();
        }
    }
}