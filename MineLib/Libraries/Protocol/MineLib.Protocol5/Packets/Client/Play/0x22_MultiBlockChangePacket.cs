using System;

using Aragas.Network.IO;

using MineLib.Core.Anvil;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class MultiBlockChangePacket : ClientPlayPacket
    {
		public Int32 ChunkX;
		public Int32 ChunkZ;
		public BlockLocation[] Records;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			ChunkX = deserialiser.Read(ChunkX);
			ChunkZ = deserialiser.Read(ChunkZ);
			var RecordsLength = deserialiser.Read<Int16>();
            var DataSize = RecordsLength * 4;
			Records = deserialiser.Read(Records, RecordsLength);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(ChunkX);
            serializer.Write(ChunkZ);
            serializer.Write((Int16) Records.Length);
            serializer.Write(Records.Length * 4);
            serializer.Write(Records, false);
        }
    }
}