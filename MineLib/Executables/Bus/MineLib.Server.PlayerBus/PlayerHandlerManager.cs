using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;

using Microsoft.Extensions.Options;

using MineLib.Server.Core.NetworkBus.Messages;

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MineLib.Server.PlayerBus
{
    public sealed class PlayerHandlerManager :
        IMessageHandler<GetExistingPlayerHandlerRequestMessage, GetExistingPlayerHandlerResponseMessage>,
        IExclusiveMessageHandler<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>,
        IDisposable
    {
        private ServiceOptions ServiceOptions { get; }
        private ConcurrentDictionary<Guid, PlayerHandler.PlayerHandler> PlayerHanlders { get; } = new ConcurrentDictionary<Guid, PlayerHandler.PlayerHandler>();

        public PlayerHandlerManager(IOptions<ServiceOptions> serviceOptions)
        {
            ServiceOptions = serviceOptions.Value;
        }

        public Task<GetExistingPlayerHandlerResponseMessage> HandleAsync(GetExistingPlayerHandlerRequestMessage message)
        {
            return Task.FromResult(
                PlayerHanlders.TryGetValue(message.PlayerId, out var playerHandler) && playerHandler.ProtocolVersion == message.ProtocolVersion
                    ? new GetExistingPlayerHandlerResponseMessage() { ServiceId = ServiceOptions.Uid, State = playerHandler.State!.Value }
                    : new GetExistingPlayerHandlerResponseMessage() { ServiceId = null });
        }

        public Task<bool> CanHandle(GetNewPlayerHandlerRequestMessage message)
        {
            return Task.FromResult(true);
        }
        public Task<GetNewPlayerHandlerResponseMessage> HandleAsync(GetNewPlayerHandlerRequestMessage message)
        {
            var stuff = new PlayerHandler.PlayerHandler(message.PlayerId, message.ProtocolVersion);
            PlayerHanlders.TryAdd(message.PlayerId, stuff);

            return Task.FromResult(new GetNewPlayerHandlerResponseMessage() { ServiceId = ServiceOptions.Uid });
        }

        private bool disposedValue = false;
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var playerHandler in PlayerHanlders)
                        playerHandler.Value.Dispose();
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