using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x03), PacketSize(1028)]
    public class LevelDataChunkPacket : ServerClassicPacket
    {
        public short ChunkLength;
        public byte[] ChunkData;
        public byte PercentComplete;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            ChunkLength = deserializer.Read(ChunkLength);
            ChunkData = deserializer.Read(ChunkData, 1024);
            PercentComplete = deserializer.Read(PercentComplete);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ChunkLength);
            serializer.Write(ChunkData);
            serializer.Write(PercentComplete);
        }
    }
}