using Aragas.Network.Attributes;
using Aragas.Network.IO;

using PokeD.Core.Data.P3D;
using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Client
{
    [Packet((int) P3DPacketTypes.Ping)]
    public class PingPacket : P3DPacket
    {
        public override void Deserialize(IPacketDeserializer deserializer) { }
        public override void Serialize(IStreamSerializer serializer) { }
    }
}
