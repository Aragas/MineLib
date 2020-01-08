using Aragas.Network.Attributes;

using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Client
{
    [Packet((int) P3DPacketTypes.Ping)]
    public class PingPacket : P3DPacket
    {
        public override void Deserialize(P3DDeserializer deserializer) { }
        public override void Serialize(P3DSerializer serializer) { }
    }
}
