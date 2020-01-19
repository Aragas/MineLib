using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Authorization
{
    public class EncryptionRequestPacket : PokeDPacket
    {
        public byte[] PublicKey { get; set; } = new byte[0];
        public byte[] VerificationToken { get; set; } = new byte[0];


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PublicKey = deserializer.Read(PublicKey);
            VerificationToken = deserializer.Read(VerificationToken);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(PublicKey);
            serializer.Write(VerificationToken);
        }
    }
}
