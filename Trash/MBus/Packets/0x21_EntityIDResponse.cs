using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Protocol.IPC.Packets
{
    [Packet(0x21)]
    public class EntityIDResponse : ProtocolPacket
    {
        public int? EntityID;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            EntityID = deserialiser.Read(EntityID);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
        }
    }
}