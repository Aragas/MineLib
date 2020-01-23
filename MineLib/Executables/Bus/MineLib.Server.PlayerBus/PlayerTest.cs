using System.Threading.Tasks;

using Aragas.QServer.Core.NetworkBus;
using MineLib.Server.Core.NetworkBus.Messages;

namespace MineLib.Server.PlayerBus
{
    public class PlayerTest :
        IMessageHandler<PlayerPositionRequestMessage, PlayerPositionResponseMessage>,
        IMessageHandler<PlayerLookRequestMessage, PlayerLookResponseMessage>
    {
        public Task<PlayerPositionResponseMessage> HandleAsync(PlayerPositionRequestMessage message)
        {
            return Task.FromResult(new PlayerPositionResponseMessage() { IsCorrect = true, Position = message.Position });
        }

        public Task<PlayerLookResponseMessage> HandleAsync(PlayerLookRequestMessage message)
        {
            return Task.FromResult(new PlayerLookResponseMessage() { IsCorrect = true, Look = message.Look });
        }
    }
}