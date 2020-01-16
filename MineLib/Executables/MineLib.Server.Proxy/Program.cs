using App.Metrics;
using App.Metrics.Health;

using Aragas.QServer.Core;
using Aragas.QServer.Core.Extensions;
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
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace MineLib.Server.Proxy
{
    public class ServiceOptions
    {
        public Guid ProgramGuid { get; } = Guid.NewGuid();
    }
    public class SubscriptionStorage : IDisposable
    {
        protected CompositeDisposable Events { get; } = new CompositeDisposable();
        protected IAsyncNetworkBus NetworkBus { get; }

        public SubscriptionStorage(IAsyncNetworkBus networkBus)
        {
            NetworkBus = networkBus;
        }

        public void RegisterHandler<TMessageRequest, TMessageResponse>(IMessageHandler<TMessageRequest, TMessageResponse> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            Events.Add(NetworkBus.RegisterHandler(handler, referenceId));
        }
        public void RegisterHandler<TMessageRequest>(IMessageHandler<TMessageRequest> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
        {
            Events.Add(NetworkBus.RegisterHandler(handler, referenceId));
        }


        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseHostProgram()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }

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
                services.AddOptions<ServiceOptions>().Configure(_ => { });
                services.Configure<MineLibOptions>(hostContext.Configuration.GetSection("MineLib"));
                var provider = services.BuildServiceProvider();
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

                var lifeTime = sp.GetRequiredService<IHostApplicationLifetime>();
                lifeTime.ApplicationStopping.Register(() => subscriptionStorage.Dispose());

                subscriptionStorage.RegisterHandler(new ServiceDiscoveryHandler(serviceOptions.ProgramGuid, "Proxy"));
                subscriptionStorage.RegisterHandler(new MetricsPrometheusHandler(sp.GetRequiredService<IMetricsRoot>()), serviceOptions.ProgramGuid);
                subscriptionStorage.RegisterHandler(new HealthHandler(sp.GetRequiredService<IHealthRoot>()), serviceOptions.ProgramGuid);

                //Events.Add(networkBus.RegisterHandler(new ServiceDiscoveryHandler(ProgramGuid, "Proxy")));
                //Events.Add(networkBus.RegisterHandler(new MetricsPrometheusHandler(sp.GetRequiredService<IMetricsRoot>()), ProgramGuid));
                //Events.Add(networkBus.RegisterHandler(new HealthHandler(sp.GetRequiredService<IHealthRoot>()), ProgramGuid));
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