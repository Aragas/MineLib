using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Core.Packets.EntityBus
{
    [Packet(0x73)]
    public sealed class EntityIDResponse : InternalPacket
    {
        public int EntityID;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            EntityID = deserializer.Read(EntityID);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(EntityID);
        }
    }
}