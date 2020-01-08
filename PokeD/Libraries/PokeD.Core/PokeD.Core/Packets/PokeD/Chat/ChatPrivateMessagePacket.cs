using Aragas.Network.Data;
using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Chat
{
    public class ChatPrivateMessagePacket : PokeDPacket
    {
        public VarInt PlayerID { get; set; }
        public string Message { get; set; } = string.Empty;


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            PlayerID = deserializer.Read(PlayerID);
            Message = deserializer.Read(Message);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(PlayerID);
            serializer.Write(Message);
        }
    }
}
