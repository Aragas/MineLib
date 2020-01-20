using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

using System;
using System.Threading.Tasks;

namespace Aragas.QServer.Core
{
    public class BaseHostProgram
    {
        public static async Task Main<TProgram>(Func<IHostBuilder, IHostBuilder>? hostBuilderFunc = null, Action<IServiceProvider>? beforeRunAction = null, string[]? args = null) where TProgram : BaseHostProgram
        {
            Aragas.Network.Extensions.PacketExtensions.Init();
            //MineLib.Core.Extensions.PacketExtensions.Init();
            //MineLib.Server.Core.Extensions.PacketExtensions.Init();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("{TypeName}: Starting.", typeof(TProgram).FullName);

                var hostBuilder = Host
                    .CreateDefaultBuilder(args ?? Array.Empty<string>())
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
                    });

                hostBuilderFunc?.Invoke(hostBuilder);

                var host = hostBuilder
                    .UseSerilog()
                    .Build();

                BeforeRun(host.Services);
                beforeRunAction?.Invoke(host.Services);

                await host.RunAsync();
            }
            catch (Exception ex) when (ex is Exception)
            {
                Log.Fatal(ex, "{TypeName}: Fatal exception.", typeof(TProgram).FullName);
            }
            finally
            {
                Log.Information("{TypeName}: Stopped.", typeof(TProgram).FullName);
                Log.CloseAndFlush();
            }
        }

        private static void BeforeRun(IServiceProvider serviceProvider)
        {
            var serviceOptions = serviceProvider.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = serviceProvider.GetRequiredService<SubscriptionStorage>();

            subscriptionStorage.HandleServiceDiscoveryHandler();
            subscriptionStorage.HandleMetricsPrometheusHandler(serviceOptions.Uid);
            subscriptionStorage.HandleHealthHandler(serviceOptions.Uid);

            var lifeTime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();
            lifeTime.ApplicationStopping.Register(() => subscriptionStorage.Dispose());
        }
    }
}