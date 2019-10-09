using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Core.Anvil;

namespace MineLib.Protocol.IPC.Packets
{
    [Packet(0x31)]
    public class WorldDataResponse : ProtocolPacket
    {
        public Chunk? Chunk;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            Chunk = deserialiser.Read(Chunk);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Chunk);
        }
    }
}