using System;

using MineLib.Protocol.Server;
using MineLib.Protocol5.Server;

namespace MineLib.Server.PlayerBus.PlayerHandler
{
    // This shit should interact via interface with multiple different protocols
    // So here we get/set info like EntityID if the protocol requests it or other shit
    internal sealed class PlayerHandler : IDisposable
    {
        public Guid PlayerId { get; }
        public int ProtocolVersion { get; }

        public int? State => ProtocolConnection?.State;

        private ProtocolConnection? ProtocolConnection { get; }

        public PlayerHandler(Guid playerId, int protocolVersion)
        {
            PlayerId = playerId;
            ProtocolVersion = protocolVersion;

            switch (protocolVersion)
            {
                case 5:
                    ProtocolConnection = new Protocol5Connection(playerId, Protocol.Netty.State.Login);
                    break;

                //case 340:
                //    ProtocolConnection = new Protocol340Connection(socket);
                //    break;

                default:
                    ProtocolConnection = null;
                    Console.WriteLine($"Protocol {protocolVersion} not available.");
                    break;
            }
        }

        public void Dispose()
        {
            ProtocolConnection?.Dispose();
        }
    }
}