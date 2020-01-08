using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Authorization
{
    public class EncryptionRequestPacket : PokeDPacket
    {
        public byte[] PublicKey { get; set; } = new byte[0];
        public byte[] VerificationToken { get; set; } = new byte[0];


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            PublicKey = deserializer.Read(PublicKey);
            VerificationToken = deserializer.Read(VerificationToken);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(PublicKey);
            serializer.Write(VerificationToken);
        }
    }
}
