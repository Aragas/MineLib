using System;
using Microsoft.Extensions.DependencyInjection;
using MineLib.Protocol.Server;
using MineLib.Protocol5.Server;
using MineLib.Protocol575.Server;

namespace MineLib.Server.PlayerBus
{

    // This shit should interact via interface with multiple different protocols
    // So here we get/set info like EntityID if the protocol requests it or other shit
    internal sealed class PlayerHandler : IDisposable
    {
        public Guid PlayerId { get; }
        public int ProtocolVersion { get; }

        public int? State => ProtocolConnection?.State;

        private ProtocolConnection? ProtocolConnection { get; }

        public PlayerHandler(IServiceProvider serviceProvider, Guid playerId, int protocolVersion)
        {
            PlayerId = playerId;
            ProtocolVersion = protocolVersion;

            switch (protocolVersion)
            {
                case 5:
                    ProtocolConnection = ActivatorUtilities.CreateInstance<Protocol5Connection>(serviceProvider, new object[] { playerId, Protocol.Netty.State.Login });
                    break;

                //case 340:
                //    ProtocolConnection = new Protocol340Connection(socket);
                //    break;

                case 498:
                    ProtocolConnection = ActivatorUtilities.CreateInstance <Protocol575Connection>(serviceProvider, new object[] { playerId, Protocol.Netty.State.Login });
                    break;

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