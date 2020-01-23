using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using NATS.Client;

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

            var configuration = new ConfigurationBuilder().AddJsonFile("loggerconfig.json").Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("{TypeName}: Starting.", typeof(TProgram).FullName);

                var hostBuilder = CreateHostBuilder(args ?? Array.Empty<string>());
                hostBuilderFunc?.Invoke(hostBuilder);

                var host = hostBuilder
                    .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration))
                    .Build();

                BeforeRun(host.Services);
                beforeRunAction?.Invoke(host.Services);

                await host.RunAsync();
            }
            catch (Exception ex) when (ex is Exception)
            {
                Log.Fatal(ex, "{TypeName}: Fatal exception.", typeof(TProgram).FullName);
                throw;
            }
            finally
            {
                Log.Information("{TypeName}: Stopped.", typeof(TProgram).FullName);
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args ?? Array.Empty<string>())
            .ConfigureHostConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddJsonFile("loggerconfig.json");
            })
            // NATS
            .ConfigureServices(services =>
            {
                services.AddSingleton<NATS.Client.Options>(ConnectionFactory.GetDefaultOptions().SetDefaultArgs());
                services.AddSingleton<IAsyncNetworkBus>(sp => new AsyncNATSBus(sp.GetRequiredService<NATS.Client.Options>()));
                services.AddSingleton<INetworkBus>(sp => sp.GetRequiredService<IAsyncNetworkBus>());
                services.AddSingleton<SubscriptionStorage>();
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
            });

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