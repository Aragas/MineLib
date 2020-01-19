using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class BlockActionPacket : ClientPlayPacket
    {
		public Int32 X;
		public Int16 Y;
		public Int32 Z;
		public Byte Byte1;
		public Byte Byte2;
		public VarInt BlockType;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Byte1 = deserializer.Read(Byte1);
			Byte2 = deserializer.Read(Byte2);
			BlockType = deserializer.Read(BlockType);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Byte1);
            serializer.Write(Byte2);
            serializer.Write(BlockType);
        }
    }
}