using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x08), PacketSize(10)]
    public class PositionAndOrientationTeleportPacket : ServerClassicPacket
    {
        public sbyte PlayerID;
        public short X;
        public short Y;
        public short Z;
        public byte Yaw;
        public byte Pitch;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PlayerID = deserializer.Read(PlayerID);
            X = deserializer.Read(X);
            Y = deserializer.Read(Y);
            Z = deserializer.Read(Z);
            Yaw = deserializer.Read(Yaw);
            Pitch = deserializer.Read(Pitch);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(PlayerID);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
        }
    }
}