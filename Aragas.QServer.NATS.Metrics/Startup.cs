using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Aragas.QServer.Core;
using Aragas.QServer.Core.NetworkBus.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aragas.QServer.NATS.Metrics
{
    public class ServiceEntry
    {
        public string ServiceType { get; set; }
        public Guid ServiceId { get; set; }
        public int NotFoundCounter { get; set; }

        public ServiceEntry(string serviceType, Guid serviceId)
        {
            ServiceType = serviceType;
            ServiceId = serviceId;
        }

        public override string ToString() => $"{ServiceType}: {ServiceId}";
    }
    public interface IPingService
    {
        List<ServiceEntry> Services { get; }
    }
    public class PingService : BackgroundService, IPingService
    {
        public List<ServiceEntry> Services { get; private set; } = new List<ServiceEntry>();

        public PingService()
        {
            BaseSingleton.Instance.Subscribe<ServicesPongMessage>(message =>
            {
                var item = Services.Find(i => i.ServiceType == message.ServiceType && i.ServiceId == message.ServiceId);
                if (item == null)
                    Services.Add(new ServiceEntry(message.ServiceType, message.ServiceId));
                else
                    item.NotFoundCounter = 0;
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var toRemove = new List<ServiceEntry>();
            while (!stoppingToken.IsCancellationRequested)
            {
                toRemove.Clear();
                foreach (var item in Services)
                {
                    item.NotFoundCounter++;
                    if (item.NotFoundCounter == 2)
                        toRemove.Add(item);
                }
                foreach (var item in toRemove)
                    Services.Remove(item);

                await BaseSingleton.Instance.PublishAsync(new ServicesPingMessage());
                await Task.Delay(2000, stoppingToken);
            }
        }
    }

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
