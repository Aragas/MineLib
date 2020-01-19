using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.QServer.Core.Packets;

using MineLib.Core.Anvil;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x37)]
    public sealed class ChunkBulkResponse : InternalPacket
    {
        public Chunk[] ChunkBulk;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            ChunkBulk = deserializer.Read(ChunkBulk);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(ChunkBulk);
        }
    }
}