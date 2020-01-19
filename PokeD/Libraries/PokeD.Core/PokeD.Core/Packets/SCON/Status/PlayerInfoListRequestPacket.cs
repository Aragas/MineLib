using Aragas.Network.IO;

namespace PokeD.Core.Packets.SCON.Status
{
    public class PlayerInfoListRequestPacket : SCONPacket
    {
        public override void Deserialize(IPacketDeserializer deserializer) { }
        public override void Serialize(IStreamSerializer serializer) { }
    }
}
