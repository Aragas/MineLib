using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using MineLib.Server.Core;

using System.Threading.Tasks;

namespace MineLib.Server.ForgeBus
{
    // This shit should do dark jni shit and bridge the whole Forge API and our bus system
    // Basically, here we run the plugin with initialization and shit. I think that we only need to retranslate
    // PluginMessage packet from Protocol to ForgeBus
    internal sealed class Program : MineLibHostProgram
    {
        public static async Task Main(string[] args)
        {
            await Main<Program>(CreateHostBuilder, null, args);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "Forge");
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

                var lifeTime = sp.GetRequiredService<IHostApplicationLifetime>();
                lifeTime.ApplicationStopping.Register(() => subscriptionStorage.Dispose());
            })

            .UseConsoleLifetime();
    }
}