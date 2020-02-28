using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;
using Aragas.QServer.NetworkBus.Messages;
using Aragas.QServer.Prometheus.Exporter.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System.Text;
using System.Threading.Tasks;

namespace Aragas.QServer.Prometheus.Exporter
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            await QServerHostProgram.Main<Program>(hostBuilderFunc: CreateHostBuilder, beforeRunAction: BeforeRun, args: args);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "Prometheus.Explorer");
            })

            .ConfigureServices(services =>
            {
                services.AddSingleton<IPingService, PingService>();
                services.AddSingleton<IHostedService, PingService>(p => (PingService) p.GetRequiredService<IPingService>());
            })

            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGet("/metrics", async context =>
                            {
                                var networkBus = context.RequestServices.GetRequiredService<IAsyncNetworkBus>();
                                var pingService = context.RequestServices.GetRequiredService<IPingService>();

                                var sb = new StringBuilder();
                                foreach (var service in pingService.Services)
                                {
                                    var response = await networkBus
                                        .PublishAndWaitForReplyAsync<AppMetricsPrometheusRequestMessage, AppMetricsPrometheusResponseMessage>(
                                            new AppMetricsPrometheusRequestMessage(),
                                            service.ServiceId);
                                    sb.AppendLine(response.Report);
                                }

                                await context.Response.WriteAsync(sb.ToString());
                            });
                        });
                    })
                    .UseKestrel();
            });

        private static void BeforeRun(IHost host)
        {
            var serviceOptions = host.Services.GetRequiredService<IOptions<ServiceOptions>>().Value;
        }
    }
}