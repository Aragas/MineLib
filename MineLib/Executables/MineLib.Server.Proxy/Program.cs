using Aragas.QServer.Core;
using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Handlers;

using I18Next.Net.Backends;
using I18Next.Net.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using MineLib.Server.Proxy.BackgroundServices;
using MineLib.Server.Proxy.Data;

using Serilog;

using System.Threading.Tasks;

namespace MineLib.Server.Proxy
{
    /// <summary>
    /// Handles connection to Player
    /// Proxy the Player connection to MineLib.Server.PlayerHandler
    /// ---
    /// Proxy is used to hide the fact that we distribute Players to multiple
    /// PlayerHandler instances. It's like BungeeCord, but internally it's
    /// one big server, not multiple small servers.
    /// ---
    /// Responses to Server List Ping (The Netty should respond to both Legacy and Netty)
    /// ---
    /// When the Player starts to Login, find first available PlayerHandler and
    /// create a Proxy connection (Player<-->Proxy<-->PlayerHandler)
    /// Send every Login/Play state packet to PlayerHandler
    /// Else, abort connection with Player
    /// If Player disconnected, disconnect PlayerHandler too.
    /// ---
    /// Because of possible encryption and compression, do not attempt
    /// to read Login/Player state packets.
    /// ---
    /// If MineLib.Proxy is down, after creating a new MineLib.Proxy process,
    /// send to the PlayerHandlerBus via broadcast to get rid of every player
    /// ---
    /// To find an available PlayerHandler, send a broadcast message
    /// that you need a connection and (maybe?) wait for every response.
    /// Based on that response, either attempt to fill the biggest PlayerHandler
    /// or fill the smallest one (Player.Count context).
    /// </summary>
    public class Program : BaseHostProgram
    {
        public static async Task Main(string[] args) => await Main<Program>(() => CreateHostBuilder(args));

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)

            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "Proxy");
                services.Configure<MineLibOptions>(hostContext.Configuration.GetSection("MineLib"));

                var mineLib = services.BuildServiceProvider().GetRequiredService<IOptions<MineLibOptions>>();
                services.AddSingleton(new ServerInfo
                {
                    Name = mineLib.Value.Name,
                    Description = mineLib.Value.Description,
                    MaxConnections = mineLib.Value.MaxConnections,
                    CurrentConnections = 0
                });
            })

            // Localization
            .ConfigureServices(services =>
            {
                services.AddI18NextLocalization(i18N => i18N.AddBackend(new JsonFileBackend("locales")));
            })

            // Metrics
            .ConfigureServices(services =>
            {
                services.AddPrometheusEndpoint();
                services.AddDefaultMetrics();
            })
            // HealthCheck
            .ConfigureServices(services =>
            {
                services.AddHealthCheckPublisher();
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

                subscriptionStorage.Handle<ServiceDiscoveryHandler>();
                subscriptionStorage.Handle<MetricsPrometheusHandler>(referenceId: serviceOptions.Uid);
                subscriptionStorage.Handle<HealthHandler>(referenceId: serviceOptions.Uid);

                var lifeTime = sp.GetRequiredService<IHostApplicationLifetime>();
                lifeTime.ApplicationStopping.Register(() => subscriptionStorage.Dispose());
            })

            // Netty Listener
            .ConfigureServices(services  =>
            {
                services.AddHostedService<ProxyNettyListenerService>();
            })

            .UseSerilog()

            .UseConsoleLifetime();
    }
}