using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Authorization
{
    public class EncryptionResponsePacket : PokeDPacket
    {
        public byte[] SharedSecret { get; set; } = new byte[0];
        public byte[] VerificationToken { get; set; } = new byte[0];


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            SharedSecret = deserializer.Read(SharedSecret);
            VerificationToken = deserializer.Read(VerificationToken);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(SharedSecret);
            serializer.Write(VerificationToken);
        }
    }
}
