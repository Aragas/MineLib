using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class BlockChangePacket : ClientPlayPacket
    {
		public Int32 X;
		public Byte Y;
		public Int32 Z;
		public VarInt BlockID;
		public Byte BlockMetadata;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			BlockID = deserializer.Read(BlockID);
			BlockMetadata = deserializer.Read(BlockMetadata);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(BlockID);
            serializer.Write(BlockMetadata);
        }
    }
}