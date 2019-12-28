using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Data;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class MapChunkBulkPacket : ProtobufPacket
    {
		public Boolean SkyLightSent;
		public Byte[] Data;
		public ChunkColumnMetadata[] MetaInformation;

        public override VarInt ID => ClientPlayPacketTypes.MapChunkBulk;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			var ChunkColumnCount = reader.Read<Int16>();
			var DataLength = reader.Read<Int32>();
			SkyLightSent = reader.Read(SkyLightSent);
			Data = reader.Read(Data, DataLength);
			MetaInformation = reader.Read(MetaInformation, ChunkColumnCount);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write((short) MetaInformation.Length);
			stream.Write(Data.Length);
			stream.Write(SkyLightSent);
			stream.Write(Data, false);
			stream.Write(MetaInformation);
          
            return this;
        }

    }
}