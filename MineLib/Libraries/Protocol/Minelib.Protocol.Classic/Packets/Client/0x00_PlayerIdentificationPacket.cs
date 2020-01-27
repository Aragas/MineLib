using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Protocol.Classic.Packets.Client
{
    [PacketID(0x00), PacketSize(130)]
    public class PlayerIdentificationPacket : ClientClassicPacket
    {
        public byte ProtocolVersion;
        public string Username;
        public string VerificationKey;
        public byte Unused;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            ProtocolVersion = deserializer.Read(ProtocolVersion);
            Username = deserializer.Read(Username);
            VerificationKey = deserializer.Read(VerificationKey);
            Unused = deserializer.Read(Unused);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ProtocolVersion);
            serializer.Write(Username);
            serializer.Write(VerificationKey);
            serializer.Write(Unused);
        }
    }
}