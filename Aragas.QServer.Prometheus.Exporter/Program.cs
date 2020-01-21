using Aragas.QServer.Core;
using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Messages;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System;
using System.Text;
using System.Threading.Tasks;

namespace Aragas.QServer.Prometheus.Exporter
{
    public sealed class Program : BaseHostProgram
    {
        public static async Task Main(string[] args)
        {
            await Main<Program>(PopulateHostBuilder, BeforeRun, args);
        }

        public static IHostBuilder PopulateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
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
                        var networkBus = app.ApplicationServices.GetRequiredService<IAsyncNetworkBus>();
                        var pingService = app.ApplicationServices.GetRequiredService<IPingService>();

                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGet("/metrics", async context =>
                            {
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
            })

            .UseConsoleLifetime();

        private static void BeforeRun(IServiceProvider serviceProvider)
        {
            var serviceOptions = serviceProvider.GetRequiredService<IOptions<ServiceOptions>>().Value;
        }
    }
}