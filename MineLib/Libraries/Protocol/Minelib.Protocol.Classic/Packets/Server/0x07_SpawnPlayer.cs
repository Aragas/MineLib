using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x07), PacketSize(74)]
    public class SpawnPlayerPacket : ServerClassicPacket
    {
        public sbyte PlayerID;
        public string PlayerName;
        public short X;
        public short Y;
        public short Z;
        public byte Yaw;
        public byte Pitch;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PlayerID = deserializer.Read(PlayerID);
            PlayerName = deserializer.Read(PlayerName);
            X = deserializer.Read(X);
            Y = deserializer.Read(Y);
            Z = deserializer.Read(Z);
            Yaw = deserializer.Read(Yaw);
            Pitch = deserializer.Read(Pitch);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(PlayerID);
            serializer.Write(PlayerName);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
        }
    }
}