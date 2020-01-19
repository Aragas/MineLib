using Aragas.Network.IO;

using PokeD.Core.Data.PokeD.Structs;

namespace PokeD.Core.Packets.PokeD.Battle
{
    /// <summary>
    /// From Client
    /// </summary>
    public class BattleAttackPacket : PokeDPacket
    {
        private MetaAttack Info { get; set; }

        public byte CurrentMonster { get => Info.CurrentMonster; set => Info = new MetaAttack(value, Move, TargetMonster); }
        public byte Move { get => Info.Move; set => Info = new MetaAttack(CurrentMonster, value, TargetMonster); }
        public byte TargetMonster { get => Info.TargetMonster; set => Info = new MetaAttack(CurrentMonster, Move, value); }


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Info = deserializer.Read(Info);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Info);
        }
    }
}
