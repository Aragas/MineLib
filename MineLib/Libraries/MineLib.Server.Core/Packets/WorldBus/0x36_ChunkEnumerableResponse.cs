using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.QServer.Core.Packets;

using MineLib.Core.Anvil;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [PacketID(0x36)]
    public sealed class ChunkEnumerableResponse : InternalPacket
    {
        public Chunk Chunk;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Chunk = deserializer.Read(Chunk);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Chunk);
        }
    }
}