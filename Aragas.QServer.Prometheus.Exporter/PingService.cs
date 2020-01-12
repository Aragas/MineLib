using Aragas.QServer.Core;
using Aragas.QServer.Core.NetworkBus.Messages;

using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Prometheus.Exporter
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
}