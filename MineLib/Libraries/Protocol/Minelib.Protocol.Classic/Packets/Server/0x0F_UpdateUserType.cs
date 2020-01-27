using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x0F), PacketSize(2)]
    public class UpdateUserTypePacket : ServerClassicPacket
    {
        public byte UserType;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            UserType = deserializer.Read(UserType);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(UserType);
        }
    }
}