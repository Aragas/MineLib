using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x06), PacketSize(8)]
    public class SetBlockPacket : ServerClassicPacket
    {
        public short X;
        public short Y;
        public short Z;
        public byte BlockType;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            X = deserializer.Read(X);
            Y = deserializer.Read(Y);
            Z = deserializer.Read(Z);
            BlockType = deserializer.Read(BlockType);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(BlockType);
        }
    }
}