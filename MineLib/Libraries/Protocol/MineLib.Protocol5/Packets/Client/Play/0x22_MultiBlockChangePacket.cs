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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			ChunkX = deserializer.Read(ChunkX);
			ChunkZ = deserializer.Read(ChunkZ);
			var RecordsLength = deserializer.Read<Int16>();
            var DataSize = RecordsLength * 4;
			Records = deserializer.Read(Records, RecordsLength);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ChunkX);
            serializer.Write(ChunkZ);
            serializer.Write((Int16) Records.Length);
            serializer.Write(Records.Length * 4);
            serializer.Write(Records, false);
        }
    }
}