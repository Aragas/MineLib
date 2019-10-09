using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Proxy.Packets.Netty.Serverbound
{
    [Packet(0xFE)]
    public sealed class ServerListPingPacket : ProxyNettyPacket // To support legacy ping
    {
        public byte Magic { get; set; }

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            Magic = deserialiser.Read(Magic);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Magic);
        }
    }
}