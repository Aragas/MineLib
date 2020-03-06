using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Messages;
using Aragas.QServer.Prometheus.Exporter.Data;

using Microsoft.Extensions.Hosting;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Prometheus.Exporter.Services
{
    public class PingService : BackgroundService, IPingService,
        IMessageReceiver<ServicesPongMessage>
    {
        public List<ServiceEntry> Services => _services.Select(kp => kp.Key).ToList();

        // There is no ConcurrentHashSet
        private readonly ConcurrentDictionary<ServiceEntry, byte> _services = new ConcurrentDictionary<ServiceEntry, byte>();
        private readonly IAsyncNetworkBus _networkBus;

        public PingService(SubscriptionStorage subscriptionStorage, IAsyncNetworkBus networkBus)
        {
            _networkBus = networkBus;

            subscriptionStorage.ReceiveServicesPong(this);
        }

        public Task HandleAsync(ServicesPongMessage message)
        {
            var (serviceEntry, _) = _services.FirstOrDefault(kp => kp.Key.ServiceType == message.ServiceType && kp.Key.ServiceId == message.ServiceId);
            if (serviceEntry == null)
                _services.TryAdd(new ServiceEntry(message.ServiceType, message.ServiceId), 0);
            else
                serviceEntry.NotFoundCounter = 0;

            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var toRemove = new List<ServiceEntry>();
            while (!stoppingToken.IsCancellationRequested)
            {
                toRemove.Clear();
                foreach (var (serviceEntry, _) in _services)
                {
                    serviceEntry.NotFoundCounter++;
                    if (serviceEntry.NotFoundCounter == 2)
                        toRemove.Add(serviceEntry);
                }
                foreach (var serviceEntryToRemove in toRemove)
                    _services.TryRemove(serviceEntryToRemove, out _);

                await _networkBus.PublishAsync(new ServicesPingMessage());
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}