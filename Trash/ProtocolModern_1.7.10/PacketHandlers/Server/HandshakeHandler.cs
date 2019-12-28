using Aragas.Core.Packets;

using MineLib.Core.Client;

using ProtocolModern.Enum;
using ProtocolModern.Packets.Server.Handshake;

namespace ProtocolModern.PacketHandlers.Server
{
    public class HandshakeHandler : ProtocolPacketHandler<HandshakePacket>
    {
        public override ProtobufPacket Handle(HandshakePacket packet)
        {
            switch ((NextState) (int) packet.NextState)
            {
                case NextState.Status:
                    Context.SetState(ClientState.InfoRequest);
                    break;

                case NextState.Login:
                    Context.SetState(ClientState.Joining);
                    break;
            }

            return null;
        }
    }
}
