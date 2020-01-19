using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Battle
{
    /// <summary>
    /// From Client
    /// </summary>
    public class BattleRequestPacket : PokeDPacket
    {
        public string Message { get; set; } = string.Empty;
        public byte Type { get; set; }
        //public IBattleInfo Battle { get; set; } = new BattleInfo1x1();


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Message = deserializer.Read(Message);
            Type = deserializer.Read(Type);

            /*
            switch ((BattleType) Type)
            {
                case BattleType.PvP_1:
                case BattleType.PvP_2:
                case BattleType.PvE_1:
                case BattleType.PvE_2:
                    Battle = new BattleInfo1x1().FromReader(reader);
                    break;

                case BattleType.PvP_3:
                case BattleType.PvP_5:
                case BattleType.PvE_3:
                case BattleType.PvE_5:
                case BattleType.PvE_6:
                case BattleType.PvE_7:
                    Battle = new BattleInfo2x2().FromReader(reader);
                    break;

                case BattleType.PvP_4:
                case BattleType.PvE_4:
                    Battle = new BattleInfo3x3().FromReader(reader);
                    break;

                case BattleType.PvE_8:
                    Battle = new BattleInfo1x5().FromReader(reader);
                    break;
            }
            */
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Message);
            serializer.Write(Type);
            /*
            Battle.ToStream(serializer);
            */
        }
    }
}
