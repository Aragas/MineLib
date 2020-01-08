using Aragas.Network.Attributes;

using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Server
{
    [Packet((int) P3DPacketTypes.Kicked)]
    public class KickedPacket : P3DPacket
    {
        public string Reason { get => DataItems[0]; set => DataItems[0] = value; }

        public override void Deserialize(P3DDeserializer deserializer) { }
        public override void Serialize(P3DSerializer serializer) { }
    }
}
