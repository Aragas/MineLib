using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Chat
{
    public class ChatServerMessagePacket : PokeDPacket
    {
        public string Message { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Message = deserializer.Read(Message);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Message);
        }
    }
}
