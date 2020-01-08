using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Battle
{
    /// <summary>
    /// From Client
    /// </summary>
    public class BattleAcceptPacket : PokeDPacket
    {
        public bool IsAccepted { get; set; }


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            IsAccepted = deserializer.Read(IsAccepted);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(IsAccepted);
        }
    }
}
