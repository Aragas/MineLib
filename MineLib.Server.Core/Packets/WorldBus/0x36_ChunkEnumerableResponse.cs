using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Core.Anvil;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x36)]
    public sealed class ChunkEnumerableResponse : InternalPacket
    {
        public Chunk Chunk;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Chunk = deserializer.Read(Chunk);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Chunk);
        }
    }
}