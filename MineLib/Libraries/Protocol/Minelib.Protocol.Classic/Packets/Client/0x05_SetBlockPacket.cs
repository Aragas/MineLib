using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Protocol.Classic.Packets.Client
{
    [PacketID(0x05), PacketSize(8)]
    public class SetBlockPacket : ClientClassicPacket
    {
        public short X;
        public short Y;
        public short Z;
        public byte Mode;
        public byte BlockType;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            X = deserializer.Read(X);
            Y = deserializer.Read(Y);
            Z = deserializer.Read(Z);
            Mode = deserializer.Read(Mode);
            BlockType = deserializer.Read(BlockType);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Mode);
            serializer.Write(BlockType);
        }
    }
}