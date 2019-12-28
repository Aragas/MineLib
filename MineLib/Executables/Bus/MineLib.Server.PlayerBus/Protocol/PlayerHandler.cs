using System;
using System.Net.Sockets;

using MineLib.Protocol.Server;
using MineLib.Protocol5.Protocol;
using MineLib.Protocol5.Server;

namespace MineLib.Server.PlayerBus.PlayerHandler
{
    // This shit should interact via interface with multiple different protocols
    // So here we get/set info like EntityID if the protocol requests it or other shit
    internal sealed class PlayerHandler : IDisposable
    {
        private ProtocolConnection? ProtocolConnection { get; }

        public PlayerHandler(Socket socket, int protocolVersion)
        {
            switch (protocolVersion)
            {
                case 5:
                    ProtocolConnection = new Protocol5Connection(socket, State.Login);
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