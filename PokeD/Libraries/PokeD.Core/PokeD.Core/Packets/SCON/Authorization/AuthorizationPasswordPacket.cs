using Aragas.Network.IO;

namespace PokeD.Core.Packets.SCON.Authorization
{
    public class AuthorizationPasswordPacket : SCONPacket
    {
        public string PasswordHash { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PasswordHash = deserializer.Read(PasswordHash);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(PasswordHash);
        }
    }
}
