using System;

using Aragas.Network.IO;

using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class MapChunkBulkPacket : ClientPlayPacket
    {
		public Boolean SkyLightSent;
		public Byte[] Data;
		public ChunkColumnMetadata[] MetaInformation;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			var ChunkColumnCount = deserialiser.Read<Int16>();
			var DataLength = deserialiser.Read<Int32>();
			SkyLightSent = deserialiser.Read(SkyLightSent);
			Data = deserialiser.Read(Data, DataLength);
			MetaInformation = deserialiser.Read(MetaInformation, ChunkColumnCount);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
			serializer.Write((short) MetaInformation.Length);
            serializer.Write(Data.Length);
            serializer.Write(SkyLightSent);
            serializer.Write(Data, false);
            serializer.Write(MetaInformation, false);
        }
    }
}