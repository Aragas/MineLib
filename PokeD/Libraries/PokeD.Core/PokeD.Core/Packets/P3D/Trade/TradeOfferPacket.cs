using Aragas.Network.Attributes;

using PokeD.Core.Data.P3D;
using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Trade
{
    [Packet((int) P3DPacketTypes.TradeOffer)]
    public class TradeOfferPacket : P3DPacket
    {
        public int DestinationPlayerID { get => int.Parse(DataItems[0] == string.Empty ? 0.ToString() : DataItems[0]); set => DataItems[0] = value.ToString(); }
        public TradeData TradeData { get => DataItems[1]; set => DataItems[1] = value; }

        public override void Deserialize(P3DDeserializer deserializer) { }
        public override void Serialize(P3DSerializer serializer) { }
    }
}
