using Aragas.Network.Attributes;
using Aragas.Network.IO;

using PokeD.Core.Data.P3D;
using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Server
{
    [Packet((int) P3DPacketTypes.DestroyPlayer)]
    public class DestroyPlayerPacket : P3DPacket
    {
        public int PlayerID { get => int.Parse(DataItems[0] == string.Empty ? 0.ToString() : DataItems[0]); set => DataItems[0] = value.ToString(); }

        public override void Deserialize(IPacketDeserializer deserializer) { }
        public override void Serialize(IPacketSerializer serializer) { }
    }
}
