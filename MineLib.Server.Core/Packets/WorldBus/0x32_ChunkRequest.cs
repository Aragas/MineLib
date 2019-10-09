using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x32)]
    public sealed class ChunkRequest : InternalPacket
    {
        public Location2D Coordinates;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Coordinates = deserializer.Read(Coordinates);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Coordinates);
        }
    }
}