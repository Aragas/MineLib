using Aragas.QServer.Core;
using Aragas.QServer.Core.NetworkBus.Messages;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Text;

namespace Aragas.QServer.Prometheus.Exporter
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPingService, PingService>();
            services.AddSingleton<IHostedService, PingService>(p => (PingService) p.GetService<IPingService>());
            //services.AddHostedService<PingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IPingService pingService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/metrics", async context =>
                {
                    var sb = new StringBuilder();
                    var services = pingService.Services;
                    foreach (var service in services)
                    {
                        var response = await BaseSingleton.Instance
                            .PublishAndWaitForReplyAsync<AppMetricsPrometheusRequestMessage, AppMetricsPrometheusResponseMessage>(new AppMetricsPrometheusRequestMessage(), service.ServiceId);
                        sb.AppendLine(response.Report);
                    }

                    await context.Response.WriteAsync(sb.ToString());
                });
            });
        }
    }
}