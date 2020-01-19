using Aragas.Network.IO;

namespace PokeD.Core.Packets.SCON.Authorization
{
    public class AuthorizationDisconnectPacket : SCONPacket
    {
        public string Reason { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Reason = deserializer.Read(Reason);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Reason);
        }
    }
}
