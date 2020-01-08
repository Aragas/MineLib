using Aragas.Network.Data;
using Aragas.Network.IO;

using PokeD.Core.Data.PokeD;

namespace PokeD.Core.Packets.PokeD.Trade
{
    public class TradeOfferPacket : PokeDPacket
    {
        public VarInt DestinationID { get; set; }
        public Monster MonsterData { get; set; } // TODO: null


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            DestinationID = deserializer.Read(DestinationID);
            MonsterData = deserializer.Read(MonsterData);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(DestinationID);
            serializer.Write(MonsterData);
        }
    }
}
