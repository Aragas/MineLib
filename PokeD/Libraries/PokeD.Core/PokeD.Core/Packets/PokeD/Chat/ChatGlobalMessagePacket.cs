using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Chat
{
    public class ChatGlobalMessagePacket : PokeDPacket
    {
        public string Message { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Message = deserializer.Read(Message);
        }
        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Message);
        }
    }
}
