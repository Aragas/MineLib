using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.QServer.Core.Packets;

using MineLib.Core;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x32)]
    public sealed class ChunkRequest : InternalPacket
    {
        public Location2D Coordinates;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Coordinates = deserializer.Read(Coordinates);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Coordinates);
        }
    }
}