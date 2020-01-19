using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Battle
{
    /// <summary>
    /// From Client
    /// </summary>
    public class BattleAcceptPacket : PokeDPacket
    {
        public bool IsAccepted { get; set; }


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            IsAccepted = deserializer.Read(IsAccepted);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(IsAccepted);
        }
    }
}
