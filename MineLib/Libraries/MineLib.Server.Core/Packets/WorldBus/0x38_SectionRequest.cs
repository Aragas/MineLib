using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.QServer.Core.Packets;

using MineLib.Core;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x38)]
    public sealed class SectionRequest : InternalPacket
    {
        // X, Z - Chunk coords, Y - Section number
        public Location3D Position;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Position = deserializer.Read(Position);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Position);
        }
    }
}