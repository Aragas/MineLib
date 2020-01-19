using Aragas.Network.Attributes;
using Aragas.Network.IO;

using PokeD.Core.Data.P3D;
using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Server
{
    [Packet((int) P3DPacketTypes.Kicked)]
    public class KickedPacket : P3DPacket
    {
        public string Reason { get => DataItems[0]; set => DataItems[0] = value; }

        public override void Deserialize(IPacketDeserializer deserializer) { }
        public override void Serialize(IPacketSerializer serializer) { }
    }
}
