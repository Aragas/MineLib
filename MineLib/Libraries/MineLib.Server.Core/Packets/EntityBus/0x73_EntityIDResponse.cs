using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.QServer.Core.Packets;

namespace MineLib.Server.Core.Packets.EntityBus
{
    [PacketID(0x73)]
    public sealed class EntityIDResponse : InternalPacket
    {
        public int EntityID;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            EntityID = deserializer.Read(EntityID);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(EntityID);
        }
    }
}