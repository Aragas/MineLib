using Aragas.Network.Data;
using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Authorization
{
    public class AuthorizationCompletePacket : PokeDPacket
    {
        public VarInt PlayerID { get; set; }

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            PlayerID = deserializer.Read(PlayerID);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(PlayerID);
        }
    }
}
