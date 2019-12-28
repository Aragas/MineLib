using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Protocol.IPC.Packets
{
    [Packet(0x20)]
    public class EntityIDRequest : ProtocolPacket
    {
        public override void Deserialize(ProtobufDeserializer deserialiser) { }

        public override void Serialize(ProtobufSerializer serializer) { }
    }
}