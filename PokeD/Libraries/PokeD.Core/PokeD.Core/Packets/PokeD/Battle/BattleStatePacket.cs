using Aragas.Network.IO;

using PokeD.BattleEngine.Battle;

namespace PokeD.Core.Packets.PokeD.Battle
{
    /// <summary>
    /// From Server
    /// </summary>
    public class BattleStatePacket : PokeDPacket
    {
        public BattleState BattleState { get; set; }


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            BattleState = deserializer.Read(BattleState);
        }
        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(BattleState);
        }
    }
}
