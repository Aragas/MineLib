using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Chat
{
    public class ChatServerMessagePacket : PokeDPacket
    {
        public string Message { get; set; } = string.Empty;


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            Message = deserializer.Read(Message);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Message);
        }
    }
}
