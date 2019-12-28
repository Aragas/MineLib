using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace PokeD.Server.Proxy.Packets.P3D.Serverbound
{
    [Packet(0xFE)]
    public sealed class ServerListPingPacket : ProxyP3DPacket // To support legacy ping
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