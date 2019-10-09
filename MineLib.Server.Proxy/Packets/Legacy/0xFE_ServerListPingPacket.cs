using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Proxy.Packets.Legacy
{
    [Packet(0xFE)]
    internal sealed class ServerListPingPacket : ProxyLegacyPacket
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