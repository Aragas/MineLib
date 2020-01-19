using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ChunkDataPacket : ClientPlayPacket
    {
		public Int32 ChunkX;
		public Int32 ChunkZ;
		public Boolean GroundUpContinuous;
		public UInt16 PrimaryBitmap;
		public UInt16 AddBitmap;
		public Byte[] CompressedData;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			ChunkX = deserializer.Read(ChunkX);
			ChunkZ = deserializer.Read(ChunkZ);
			GroundUpContinuous = deserializer.Read(GroundUpContinuous);
			PrimaryBitmap = deserializer.Read(PrimaryBitmap);
			AddBitmap = deserializer.Read(AddBitmap);
			var CompressedSize = deserializer.Read<Int32>();
			CompressedData = deserializer.Read(CompressedData, CompressedSize);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ChunkX);
            serializer.Write(ChunkZ);
            serializer.Write(GroundUpContinuous);
            serializer.Write(PrimaryBitmap);
            serializer.Write(AddBitmap);
            serializer.Write(CompressedData.Length);
            serializer.Write(CompressedData, false);
        }
    }
}