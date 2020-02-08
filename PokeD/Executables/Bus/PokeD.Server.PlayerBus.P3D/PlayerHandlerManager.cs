using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using PokeD.Server.Core.Data;
using PokeD.Server.Core.NetworkBus.Messages;
using PokeD.Server.PlayerBus.P3D;

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace PokeD.Server.PlayerBus
{
    public sealed class PlayerHandlerManager :
        IMessageHandler<GetExistingPlayerHandlerRequestMessage, GetExistingPlayerHandlerResponseMessage>,
        IExclusiveMessageHandler<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>,
        IDisposable
    {
        private int MaxPlayers = 20;
        private IServiceProvider ServiceProvider { get; }
        private ServiceOptions ServiceOptions { get; }
        private ConcurrentDictionary<Guid, P3DPlayer> PlayerHandlers { get; } = new ConcurrentDictionary<Guid, P3DPlayer>();

        public PlayerHandlerManager(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceOptions = serviceProvider.GetRequiredService<IOptions<ServiceOptions>>().Value;
        }

        public Task<GetExistingPlayerHandlerResponseMessage> HandleAsync(GetExistingPlayerHandlerRequestMessage message)
        {
            return Task.FromResult(
                PlayerHandlers.TryGetValue(message.PlayerId, out var playerHandler) && message.PlayerType == PlayerType.P3D
                    ? new GetExistingPlayerHandlerResponseMessage() { ServiceId = ServiceOptions.Uid }
                    : new GetExistingPlayerHandlerResponseMessage() { ServiceId = null });
        }

        public Task<bool> CanHandle(GetNewPlayerHandlerRequestMessage message)
        {
            return Task.FromResult(PlayerHandlers.Count < MaxPlayers);
        }
        public Task<GetNewPlayerHandlerResponseMessage> HandleAsync(GetNewPlayerHandlerRequestMessage message)
        {
            var playerHandler = ActivatorUtilities.CreateInstance<P3DPlayer>(ServiceProvider, new object[] { message.PlayerId });
            PlayerHandlers.TryAdd(message.PlayerId, playerHandler);

            return Task.FromResult(new GetNewPlayerHandlerResponseMessage() { ServiceId = ServiceOptions.Uid });
        }

        public void Dispose()
        {
            foreach (var playerHandler in PlayerHandlers)
                playerHandler.Value.Dispose();
        }
    }
}