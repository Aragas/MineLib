using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

using System;
using System.Threading.Tasks;

namespace Aragas.QServer.Core
{
    public class BaseHostProgram
    {
        public static async Task Main<TProgram>(Func<IHostBuilder, IHostBuilder>? hostBuilderFunc = null, string[]? args = null) where TProgram : BaseHostProgram
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
                    });

                hostBuilderFunc?.Invoke(hostBuilder);

                await hostBuilder
                    .UseSerilog()
                    .Build()
                    .RunAsync();
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
    }
}