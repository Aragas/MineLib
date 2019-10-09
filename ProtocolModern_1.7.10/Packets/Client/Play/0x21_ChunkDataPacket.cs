using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class ChunkDataPacket : ProtobufPacket
    {
		public Int32 ChunkX;
		public Int32 ChunkZ;
		public Boolean GroundUpContinuous;
		public UInt16 PrimaryBitmap;
		public UInt16 AddBitmap;
		public Byte[] CompressedData;

        public override VarInt ID => ClientPlayPacketTypes.ChunkData;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			ChunkX = reader.Read(ChunkX);
			ChunkZ = reader.Read(ChunkZ);
			GroundUpContinuous = reader.Read(GroundUpContinuous);
			PrimaryBitmap = reader.Read(PrimaryBitmap);
			AddBitmap = reader.Read(AddBitmap);
			var CompressedSize = reader.Read<Int32>();
			CompressedData = reader.Read(CompressedData, CompressedSize);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(ChunkX);
			stream.Write(ChunkZ);
			stream.Write(GroundUpContinuous);
			stream.Write(PrimaryBitmap);
			stream.Write(AddBitmap);
			stream.Write(CompressedData.Length);
			stream.Write(CompressedData, false);
          
            return this;
        }

    }
}