using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x38)]
    public sealed class SectionRequest : InternalPacket
    {
        // X, Z - Chunk coords, Y - Section number
        public Location3D Position;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Position = deserializer.Read(Position);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Position);
        }
    }
}