using Aragas.QServer.Logging.Serilog;
using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Loki;

using System;
using System.Threading.Tasks;

namespace Aragas.QServer.Hosting
{
    public static class QServerHostProgram
    {
        public static Guid Uid { get; } = Guid.NewGuid();

        public static async Task Main<TProgram>(
            Func<IHostBuilder, IHostBuilder>? hostBuilderFunc = null,
            Action<IHost>? beforeRunAction = null,
            string[]? args = null)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("loggerconfig.json", false)
                .AddJsonFile($"loggerconfig.{env}.json", true)
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithApplicationInfo(Uid)
                //.Enrich.WithLogLevel()
                //.WriteTo.LokiHttp(new NoAuthCredentials("http://ssh.khadas.aragas.org:43100"))
                .CreateLogger();

            try
            {
                Log.Information("{TypeName}: Starting.", typeof(TProgram).FullName);

                var hostBuilder = QServerHost.CreateDefaultBuilder(args ?? Array.Empty<string>(), Uid);
                hostBuilderFunc?.Invoke(hostBuilder);

                var host = hostBuilder.Build();

                BeforeRun(host);
                beforeRunAction?.Invoke(host);

                await host.RunAsync();
            }
            catch (Exception ex)
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

        private static void BeforeRun(IHost host)
        {
            var serviceOptions = host.Services.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = host.Services.GetRequiredService<SubscriptionStorage>();

            subscriptionStorage.HandleServiceDiscoveryHandler();
            subscriptionStorage.HandleMetricsPrometheusHandler(serviceOptions.Uid);
            subscriptionStorage.HandleHealthHandler(serviceOptions.Uid);

            var lifeTime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            lifeTime.ApplicationStopping.Register(() => subscriptionStorage.Dispose());
        }
    }
}