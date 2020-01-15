using Aragas.QServer.Core.NetworkBus.Messages;

using System;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.NetworkBus.Handlers
{
    public class ServiceDiscoveryHandler : IMessageHandler<ServicesPingMessage, ServicesPongMessage>
    {
        private Guid ServiceId { get; }
        private string ServiceType { get; }

        public ServiceDiscoveryHandler(Guid serviceId, string serviceType)
        {
            ServiceId = serviceId;
            ServiceType = serviceType;
        }

        public Task<ServicesPongMessage> HandleAsync(ServicesPingMessage message) => Task.FromResult(new ServicesPongMessage() { ServiceId = ServiceId, ServiceType = ServiceType });
    }
}