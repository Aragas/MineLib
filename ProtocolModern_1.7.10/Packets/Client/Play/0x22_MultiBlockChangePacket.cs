using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using MineLib.Core.Data;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class MultiBlockChangePacket : ProtobufPacket
    {
		public Int32 ChunkX;
		public Int32 ChunkZ;
		public BlockPosition[] Records;

        public override VarInt ID => ClientPlayPacketTypes.MultiBlockChange;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			ChunkX = reader.Read(ChunkX);
			ChunkZ = reader.Read(ChunkZ);
			var RecordsLength = reader.Read<Int16>();
            var DataSize = RecordsLength * 4;
			Records = reader.Read(Records, RecordsLength);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(ChunkX);
			stream.Write(ChunkZ);
			stream.Write((Int16) Records.Length);
			stream.Write(Records.Length * 4);
			stream.Write(Records, false);
          
            return this;
        }

    }
}