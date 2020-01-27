using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Protocol.Netty.Packets.Server.Status
{
    [PacketID(0x00)]
    public class RequestPacket : ServerStatusPacket
    {
        public override void Deserialize(IPacketDeserializer deserializer) { }

        public override void Serialize(IPacketSerializer serializer) { }
    }
}