using Aragas.Network.IO;

namespace PokeD.Core.Packets.SCON.Authorization
{
    public class EncryptionRequestPacket : SCONPacket
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
