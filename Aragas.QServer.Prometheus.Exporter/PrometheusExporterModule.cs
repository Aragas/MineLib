using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;
using Aragas.QServer.NetworkBus.Messages;
using Aragas.QServer.Prometheus.Exporter.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Text;

using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Aragas.QServer.Prometheus.Exporter
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(QServerModule))]
    public class PrometheusExporterModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.Configure<ServiceOptions>(o => o.Name = "Prometheus.Explorer");

            services.AddSingleton<IPingService, PingService>();
            services.AddSingleton<IHostedService, PingService>(p => (PingService)p.GetRequiredService<IPingService>());
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/metrics", async ctx =>
                {
                    var networkBus = ctx.RequestServices.GetRequiredService<IAsyncNetworkBus>();
                    var pingService = ctx.RequestServices.GetRequiredService<IPingService>();

                    var sb = new StringBuilder();
                    foreach (var service in pingService.Services)
                    {
                        var response = await networkBus
                            .PublishAndWaitForReplyAsync<AppMetricsPrometheusRequestMessage, AppMetricsPrometheusResponseMessage>(
                                new AppMetricsPrometheusRequestMessage(),
                                service.ServiceId);
                        sb.AppendLine(response.Report);
                    }

                    await ctx.Response.WriteAsync(sb.ToString());
                });
            });
        }
    }
}