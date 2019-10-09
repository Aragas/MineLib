using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Core;

namespace MineLib.Protocol.IPC.Packets
{
    [Packet(0x30)]
    public class WorldDataRequest : ProtocolPacket
    {
        public Coordinates2D ChunkCoordinates;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            ChunkCoordinates = deserialiser.Read(ChunkCoordinates);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(ChunkCoordinates);
        }
    }
}