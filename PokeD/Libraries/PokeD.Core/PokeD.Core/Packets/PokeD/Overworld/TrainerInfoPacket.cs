using Aragas.Network.Data;
using Aragas.Network.IO;

using PokeD.BattleEngine.Trainer.Data;
using PokeD.Core.Extensions;

namespace PokeD.Core.Packets.PokeD.Overworld
{
    public class MetaTrainerInfo
    {
        public byte Meta { get; private set; }
        

        /// <summary>
        /// Index of used Monster.
        /// </summary>
        /// <remarks>Range 0-3, used 0-1.</remarks>
        public byte CurrentMonster { get => Meta.BitsGet(0, 2); set => Meta = Meta.BitsSet(value, 0, 2); }

        /// <summary>
        /// Index of used Move.
        /// </summary>
        /// <remarks>Range 0-3, used 0-3.</remarks>
        public byte Move { get => Meta.BitsGet(2, 4); set => Meta = Meta.BitsSet(value, 2, 4); }

        /// <summary>
        /// Index of used Monster. 16 is All. 15 is All except Attacker
        /// </summary>
        /// <remarks>Range 0-15, used 0-15.</remarks>
        public byte TargetMonster { get => Meta.BitsGet(4, 8); set => Meta = Meta.BitsSet(value, 4, 8); }


        public MetaTrainerInfo(byte currentMonster, byte move, byte targetMonster) { CurrentMonster = currentMonster; Move = move; TargetMonster = targetMonster; }
        public MetaTrainerInfo(byte meta) { Meta = meta; }
    }

    public class TrainerInfoPacket : PokeDPacket
    {
        public VarInt PlayerID { get; set; }
        public short TrainerSprite { get; set; }
        public string Name { get; set; } = string.Empty;

        public short TrainerID { get; set; }
        public byte Gender { get; set; }

        public MonsterTeam MonsterTeam { get; set; } = new MonsterTeam();


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PlayerID = deserializer.Read(PlayerID);
            TrainerSprite = deserializer.Read(TrainerSprite);
            Name = deserializer.Read(Name);

            TrainerID = deserializer.Read(TrainerID);
            Gender = deserializer.Read(Gender);

            MonsterTeam = deserializer.Read(MonsterTeam);
        }
        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(PlayerID);
            serializer.Write(TrainerSprite);
            serializer.Write(Name);

            serializer.Write(TrainerID);
            serializer.Write(Gender);

            serializer.Write(MonsterTeam);
        }
    }
}