using Aragas.Network.Data;
using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Battle
{
    /// <summary>
    /// From Server
    /// </summary>
    public class BattleOfferPacket : PokeDPacket
    {
        public VarInt[] PlayerIDs { get; set; } = new VarInt[0];
        public string Message { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PlayerIDs = deserializer.Read(PlayerIDs);
            Message = deserializer.Read(Message);
        }
        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(PlayerIDs);
            serializer.Write(Message);
        }
    }
}
