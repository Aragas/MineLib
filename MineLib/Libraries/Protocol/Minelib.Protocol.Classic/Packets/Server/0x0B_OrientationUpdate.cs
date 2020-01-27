using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x0B), PacketSize(4)]
    public class OrientationUpdatePacket : ServerClassicPacket
    {
        public sbyte PlayerID;
        public byte Yaw;
        public byte Pitch;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PlayerID = deserializer.Read(PlayerID);
            Yaw = deserializer.Read(Yaw);
            Pitch = deserializer.Read(Pitch);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(PlayerID);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
        }
    }
}