using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Messages;

using Microsoft.Extensions.Hosting;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Prometheus.Exporter
{
    public class PingService : BackgroundService, IPingService, IMessageReceiver<ServicesPongMessage>
    {
        public List<ServiceEntry> Services => _services.Select(kp => kp.Key).ToList();

        private readonly ConcurrentDictionary<ServiceEntry, object> _services = new ConcurrentDictionary<ServiceEntry, object>();
        private readonly IAsyncNetworkBus _networkBus;

        public PingService(SubscriptionStorage subscriptionStorage, IAsyncNetworkBus networkBus)
        {
            _networkBus = networkBus;

            subscriptionStorage.ReceiveServicesPong(this);
        }

        public Task HandleAsync(ServicesPongMessage message)
        {
            var item = _services.FirstOrDefault(kp => kp.Key.ServiceType == message.ServiceType && kp.Key.ServiceId == message.ServiceId);
            if (item.Key == null)
                _services.TryAdd(new ServiceEntry(message.ServiceType, message.ServiceId), null);
            else
                item.Key.NotFoundCounter = 0;

            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var toRemove = new List<ServiceEntry>();
            while (!stoppingToken.IsCancellationRequested)
            {
                toRemove.Clear();
                foreach (var item in _services)
                {
                    item.Key.NotFoundCounter++;
                    if (item.Key.NotFoundCounter == 2)
                        toRemove.Add(item.Key);
                }
                foreach (var item in toRemove)
                    _services.TryRemove(item, out _);

                await _networkBus.PublishAsync(new ServicesPingMessage());
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}