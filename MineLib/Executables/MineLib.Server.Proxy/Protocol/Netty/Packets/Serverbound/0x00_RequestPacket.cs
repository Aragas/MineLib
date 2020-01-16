using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets.Serverbound
{
    [Packet(0x00)]
    public sealed class RequestPacket : StatusStatePacket
    {
        public override void Deserialize(ProtobufDeserializer deserialiser) { }

        public override void Serialize(ProtobufSerializer serializer) { }
    }
}