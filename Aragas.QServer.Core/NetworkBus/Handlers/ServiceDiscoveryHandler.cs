using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus.Messages;

using Microsoft.Extensions.Options;

using System;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.NetworkBus.Handlers
{
    public class ServiceDiscoveryHandler : IMessageHandler<ServicesPingMessage, ServicesPongMessage>
    {
        private Guid ServiceId { get; }
        private string ServiceName { get; }

        public ServiceDiscoveryHandler(IOptions<ServiceOptions> service)
        {
            ServiceId = service.Value.Uid;
            ServiceName = service.Value.Name;
        }

        public Task<ServicesPongMessage> HandleAsync(ServicesPingMessage message) => Task.FromResult(new ServicesPongMessage() { ServiceId = ServiceId, ServiceType = ServiceName });
    }
}