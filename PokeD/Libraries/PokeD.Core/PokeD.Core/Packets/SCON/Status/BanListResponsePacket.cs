using Aragas.Network.IO;

using PokeD.Core.Data.SCON;

namespace PokeD.Core.Packets.SCON.Status
{
    public class BanListResponsePacket : SCONPacket
    {
        public Ban[] Bans { get; set; } = new Ban[0];


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            Bans = deserializer.Read(Bans);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Bans);
        }
    }
}
