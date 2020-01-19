using Aragas.Network.Attributes;
using Aragas.Network.IO;

using PokeD.Core.Data.P3D;
using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Battle
{
    [Packet((int) P3DPacketTypes.BattleHostData)]
    public class BattleHostDataPacket : P3DPacket
    {
        public int DestinationPlayerID { get => int.Parse(DataItems[0] == string.Empty ? 0.ToString() : DataItems[0]); set => DataItems[0] = value.ToString(); }
        public BattleHostData BattleData { get => DataItems[1]; set => DataItems[1] = value; }

        public override void Deserialize(IPacketDeserializer deserializer) { }
        public override void Serialize(IStreamSerializer serializer) { }
    }
}
