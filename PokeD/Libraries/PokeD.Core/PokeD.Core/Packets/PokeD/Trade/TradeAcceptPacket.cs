using Aragas.Network.Data;
using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Trade
{
    public class TradeAcceptPacket : PokeDPacket
    {
        public VarInt DestinationID { get; set; }


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            DestinationID = deserializer.Read(DestinationID);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(DestinationID);
        }
    }
}
