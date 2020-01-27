using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x00), PacketSize(131)]
    public class ServerIdentificationPacket : ServerClassicPacket
    {
        public byte ProtocolVersion;
        public string ServerName;
        public string ServerMOTD;
        public byte UserType;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            ProtocolVersion = deserializer.Read(ProtocolVersion);
            ServerName = deserializer.Read(ServerName);
            ServerMOTD = deserializer.Read(ServerMOTD);
            UserType = deserializer.Read(UserType);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ProtocolVersion);
            serializer.Write(ServerName);
            serializer.Write(ServerMOTD);
            serializer.Write(UserType);
        }
    }
}