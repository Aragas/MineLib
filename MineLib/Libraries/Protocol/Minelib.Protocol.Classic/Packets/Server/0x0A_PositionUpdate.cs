using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x0A), PacketSize(5)]
    public class PositionUpdatePacket : ServerClassicPacket
    {
        public sbyte PlayerID;
        public short X;
        public short Y;
        public short Z;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PlayerID = deserializer.Read(PlayerID);
            X = deserializer.Read(X);
            Y = deserializer.Read(Y);
            Z = deserializer.Read(Z);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(PlayerID);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
        }
    }
}