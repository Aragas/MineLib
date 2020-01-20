using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;

using Microsoft.Extensions.Options;

using MineLib.Server.Core.NetworkBus.Messages;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.EntityBus
{
    public sealed class EntityHandlerManager :
        IMessageHandler<GetNewEntityIdRequestMessage, GetNewEntityIdResponseMessage>,
        IDisposable
    {
        private ServiceOptions ServiceOptions { get; }
        private int _entityIdCounter;

        public EntityHandlerManager(IOptions<ServiceOptions> serviceOptions)
        {
            ServiceOptions = serviceOptions.Value;
        }

        public Task<GetNewEntityIdResponseMessage> HandleAsync(GetNewEntityIdRequestMessage message)
        {
            Interlocked.Increment(ref _entityIdCounter);
            return Task.FromResult(new GetNewEntityIdResponseMessage() { EntityId = _entityIdCounter });
        }

        private bool disposedValue = false;
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}