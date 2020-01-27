using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace Aragas.QServer.Core.Packets.MBus
{
    [PacketID(0x10)]
    public sealed class SubscribeRequest : InternalPacket
    {
        public string Name = string.Empty;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Name = deserializer.Read(Name);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Name);
        }
    }
}