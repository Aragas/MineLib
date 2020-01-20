using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;

using Microsoft.Extensions.DependencyInjection;
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
        private int MaxPlayers = 20;
        private IServiceProvider ServiceProvider { get; }
        private ServiceOptions ServiceOptions { get; }
        private ConcurrentDictionary<Guid, PlayerHandler> PlayerHandlers { get; } = new ConcurrentDictionary<Guid, PlayerHandler>();

        public PlayerHandlerManager(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceOptions = serviceProvider.GetRequiredService<IOptions<ServiceOptions>>().Value;
        }

        public Task<GetExistingPlayerHandlerResponseMessage> HandleAsync(GetExistingPlayerHandlerRequestMessage message)
        {
            return Task.FromResult(
                PlayerHandlers.TryGetValue(message.PlayerId, out var playerHandler) && playerHandler.ProtocolVersion == message.ProtocolVersion
                    ? new GetExistingPlayerHandlerResponseMessage() { ServiceId = ServiceOptions.Uid, State = playerHandler.State!.Value }
                    : new GetExistingPlayerHandlerResponseMessage() { ServiceId = null });
        }

        public Task<bool> CanHandle(GetNewPlayerHandlerRequestMessage message)
        {
            return Task.FromResult(PlayerHandlers.Count < MaxPlayers);
        }
        public Task<GetNewPlayerHandlerResponseMessage> HandleAsync(GetNewPlayerHandlerRequestMessage message)
        {
            var playerHandler = ActivatorUtilities.CreateInstance<PlayerHandler>(ServiceProvider, new object[] { message.PlayerId, message.ProtocolVersion });
            PlayerHandlers.TryAdd(message.PlayerId, playerHandler);

            return Task.FromResult(new GetNewPlayerHandlerResponseMessage() { ServiceId = ServiceOptions.Uid });
        }

        private bool disposedValue = false;
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var playerHandler in PlayerHandlers)
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