using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Core.Anvil;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x37)]
    public sealed class ChunkBulkResponse : InternalPacket
    {
        public Chunk[] ChunkBulk;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            ChunkBulk = deserializer.Read(ChunkBulk);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(ChunkBulk);
        }
    }
}