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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			var ChunkColumnCount = deserializer.Read<Int16>();
			var DataLength = deserializer.Read<Int32>();
			SkyLightSent = deserializer.Read(SkyLightSent);
			Data = deserializer.Read(Data, DataLength);
			MetaInformation = deserializer.Read(MetaInformation, ChunkColumnCount);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
			serializer.Write((short) MetaInformation.Length);
            serializer.Write(Data.Length);
            serializer.Write(SkyLightSent);
            serializer.Write(Data, false);
            serializer.Write(MetaInformation, false);
        }
    }
}