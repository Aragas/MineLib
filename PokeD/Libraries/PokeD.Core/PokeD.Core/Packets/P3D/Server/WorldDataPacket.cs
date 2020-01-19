using Aragas.Network.Attributes;
using Aragas.Network.IO;

using PokeD.Core.Data.P3D;
using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Server
{
    [Packet((int) P3DPacketTypes.WorldData)]
    public class WorldDataPacket : P3DPacket
    {
        public int Season { get => int.Parse(DataItems[0] == string.Empty ? 0.ToString() : DataItems[0]); set => DataItems[0] = value.ToString(); }
        public int Weather { get => int.Parse(DataItems[1] == string.Empty ? 0.ToString() : DataItems[1]); set => DataItems[1] = value.ToString(); }
        public string CurrentTime { get => DataItems[2]; set => DataItems[2] = value; }

        public override void Deserialize(IPacketDeserializer deserializer) { }
        public override void Serialize(IStreamSerializer serializer) { }
    }
}
