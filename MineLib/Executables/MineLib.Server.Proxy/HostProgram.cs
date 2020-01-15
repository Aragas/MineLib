using App.Metrics;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;
using App.Metrics.Health;

using Aragas.QServer.Core;
using Aragas.QServer.Core.AppMetrics;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Messages;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MineLib.Server.Proxy.BackgroundServices;

using Serilog;

using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace MineLib.Server.Proxy
{
    public class ServiceDiscoveryHandler : IMessageHandler<ServicesPingMessage, ServicesPongMessage>
    {
        private Guid ServiceId { get; }
        private string ServiceType { get; }

        public ServiceDiscoveryHandler(Guid serviceId, string serviceType)
        {
            ServiceId = serviceId;
            ServiceType = serviceType;
        }

        public Task<ServicesPongMessage> HandleAsync(ServicesPingMessage message) => Task.FromResult(new ServicesPongMessage() { ServiceId = ServiceId, ServiceType = ServiceType });
    }
    public class AppMetricsPrometheusHandler : IMessageHandler<AppMetricsPrometheusRequestMessage, AppMetricsPrometheusResponseMessage>
    {
        private readonly IMetricsRoot _metricsRoot;
        private readonly IMetricsOutputFormatter _formatter;

        public AppMetricsPrometheusHandler(IMetricsRoot metricsRoot)
        {
            _metricsRoot = metricsRoot;
            _formatter = _metricsRoot.OutputMetricsFormatters
                .OfType<MetricsPrometheusTextOutputFormatter>()
                .SingleOrDefault();
            if (_formatter == null)
                throw new ArgumentException("Include App.Metrics.Formatters.Prometheus!", nameof(metricsRoot));
        }

        public async Task<AppMetricsPrometheusResponseMessage> HandleAsync(AppMetricsPrometheusRequestMessage message)
        {
            var snapshot = _metricsRoot.Snapshot.Get();
            using var stream = new MemoryStream();
            await _formatter.WriteAsync(stream, snapshot);
            return new AppMetricsPrometheusResponseMessage(Encoding.UTF8.GetString(stream.ToArray()));
        }
    }
    public class PlayerDataToProxyReceiver : IMessageReceiver<PlayerDataToProxyMessage>
    {
        public Task HandleAsync(PlayerDataToProxyMessage message)
        {
            ;

            return Task.CompletedTask;
        }
    }
    public class PlayerDataToBusReceiver : IMessageReceiver<PlayerDataToBusMessage>
    {
        public Task HandleAsync(PlayerDataToBusMessage message)
        {
            ;

            return Task.CompletedTask;
        }
    }

    public class HostProgram : BaseHostProgram
    {
        public static async Task Main(string[] args) => await Main<HostProgram>(args);

        protected CompositeDisposable Events { get; } = new CompositeDisposable();

        public void ConfugureSubscribtions(IServiceProvider sp)
        {
            var networkBus = sp.GetRequiredService<IAsyncNetworkBus>();

            Events.Add(networkBus.RegisterHandler(new ServiceDiscoveryHandler(ProgramGuid, "Proxy")));
            Events.Add(networkBus.RegisterHandler(new AppMetricsPrometheusHandler(sp.GetRequiredService<IMetricsRoot>()), ProgramGuid));
            Events.Add(networkBus.RegisterHandler(new AppMetricsHealthHandler(sp.GetRequiredService<IHealthRoot>()), ProgramGuid));
            Events.Add(networkBus.RegisterReceiver(new PlayerDataToProxyReceiver(), ProgramGuid));
            Events.Add(networkBus.RegisterReceiver(new PlayerDataToBusReceiver(), ProgramGuid));
        }

        public override IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            // Metrics
            .ConfigureServices((hostContext, services) =>
            {
                services.AddPrometheusEndpoint();
                services.AddDefaultMetrics();
            })
            // HealthCheck
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHealthCheckPublisher();
            })

            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IAsyncNetworkBus>(new AsyncNATSBus());
                services.AddSingleton<INetworkBus>(sp => sp.GetRequiredService<IAsyncNetworkBus>());
                ConfugureSubscribtions(services.BuildServiceProvider());
            })

            /*
            // Metrics and Health HTTP Endpoints
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseKestrel(o => o.AllowSynchronousIO = true)
                    .Configure(app =>
                    {
                        app.UseMetricsEndpoint();
                        app.UseHealthEndpoint();
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddMetricsEndpoints();
                    });
            })
            */

            // Netty Listener
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ProxyNettyListenerService>();
            })
            .UseSerilog();


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Events.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}