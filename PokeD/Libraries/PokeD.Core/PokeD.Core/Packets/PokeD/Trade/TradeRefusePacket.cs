using Aragas.Network.Data;
using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Trade
{
    public class TradeRefusePacket : PokeDPacket
    {
        public VarInt DestinationID { get; set; }


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            DestinationID = deserializer.Read(DestinationID);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(DestinationID);
        }
    }
}
