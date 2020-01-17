using Aragas.QServer.Core;
using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System.Threading.Tasks;

namespace MineLib.Server.EntityBus
{
    public class Program : BaseHostProgram
    {
        public static async Task Main(string[] args)
        {
            MineLib.Server.Core.Extensions.PacketExtensions.Init();
            MineLib.Core.Extensions.PacketExtensions.Init();
            await Main<Program>(CreateHostBuilder, args);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "EntityBus");
            })

            // Metrics
            .ConfigureServices((hostContext, services) =>
            {
                services.AddNpgSqlMetrics("Database", hostContext.Configuration["PostgreSQLConnectionString"]);
            })

            // NATS
            .ConfigureServices(services =>
            {
                services.AddSingleton<IAsyncNetworkBus>(new AsyncNATSBus());
                services.AddSingleton<INetworkBus>(sp => sp.GetRequiredService<IAsyncNetworkBus>());
                services.AddSingleton<SubscriptionStorage>();

                var sp = services.BuildServiceProvider();
                var networkBus = sp.GetRequiredService<IAsyncNetworkBus>();
                var serviceOptions = sp.GetRequiredService<IOptions<ServiceOptions>>().Value;
                var subscriptionStorage = sp.GetRequiredService<SubscriptionStorage>();

                subscriptionStorage.HandleServiceDiscoveryHandler();
                subscriptionStorage.HandleMetricsPrometheusHandler(serviceOptions.Uid);
                subscriptionStorage.HandleHealthHandler(serviceOptions.Uid);

                services.AddSingleton<EntityHandlerManager>();
                subscriptionStorage.HandleGetNewEntityId<EntityHandlerManager>();

                var lifeTime = sp.GetRequiredService<IHostApplicationLifetime>();
                lifeTime.ApplicationStopping.Register(() => subscriptionStorage.Dispose());
            })

            .UseConsoleLifetime();
    }
}