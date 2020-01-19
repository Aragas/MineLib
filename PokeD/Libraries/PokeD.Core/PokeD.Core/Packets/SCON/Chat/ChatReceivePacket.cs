using Aragas.Network.IO;

namespace PokeD.Core.Packets.SCON.Chat
{
    public class ChatReceivePacket : SCONPacket
    {
        public bool Enabled { get; set; }


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Enabled = deserializer.Read(Enabled);
        }
        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Enabled);
        }
    }
}
