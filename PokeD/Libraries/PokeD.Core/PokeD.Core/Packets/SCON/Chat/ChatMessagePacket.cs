using Aragas.Network.IO;

namespace PokeD.Core.Packets.SCON.Chat
{
    public class ChatMessagePacket : SCONPacket
    {
        public string Player { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            Player = deserializer.Read(Player);
            Message = deserializer.Read(Message);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Player);
            serializer.Write(Message);
        }
    }
}
