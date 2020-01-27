using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x04), PacketSize(7)]
    public class LevelFinalizePacket : ServerClassicPacket
    {
        public short X;
        public short Y;
        public short Z;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            X = deserializer.Read(X);
            Y = deserializer.Read(Y);
            Z = deserializer.Read(Z);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
        }
    }
}