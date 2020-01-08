using Aragas.Network.Attributes;

using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Battle
{
    [Packet((int) P3DPacketTypes.BattleJoin)]
    public class BattleJoinPacket : P3DPacket
    {
        public int DestinationPlayerID { get => int.Parse(DataItems[0] == string.Empty ? 0.ToString() : DataItems[0]); set => DataItems[0] = value.ToString(); }

        public override void Deserialize(P3DDeserializer deserializer) { }
        public override void Serialize(P3DSerializer serializer) { }
    }
}
