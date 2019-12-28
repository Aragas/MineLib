using Aragas.Core.Packets;

using ProtocolModern.Packets.Client.Play;
using ProtocolModern.Packets.Server.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class KeepAliveHandler : ProtocolPacketHandler<KeepAlivePacket>
    {
        public override ProtobufPacket Handle(KeepAlivePacket packet)
        {
            return new KeepAlive2Packet { KeepAliveID = packet.KeepAliveID };
        }
    }
}
