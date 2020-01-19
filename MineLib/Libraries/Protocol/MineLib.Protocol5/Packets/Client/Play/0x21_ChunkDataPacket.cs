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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			ChunkX = deserialiser.Read(ChunkX);
			ChunkZ = deserialiser.Read(ChunkZ);
			GroundUpContinuous = deserialiser.Read(GroundUpContinuous);
			PrimaryBitmap = deserialiser.Read(PrimaryBitmap);
			AddBitmap = deserialiser.Read(AddBitmap);
			var CompressedSize = deserialiser.Read<Int32>();
			CompressedData = deserialiser.Read(CompressedData, CompressedSize);
        }

        public override void Serialize(IStreamSerializer serializer)
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