using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x01), PacketSize(1)]
    public class PingPacket : ServerClassicPacket
    {
        public override void Deserialize(IPacketDeserializer deserializer) { }

        public override void Serialize(IPacketSerializer serializer) { }
    }
}