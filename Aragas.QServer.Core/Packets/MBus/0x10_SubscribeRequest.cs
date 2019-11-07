using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace Aragas.QServer.Core.Packets.MBus
{
    [Packet(0x10)]
    public sealed class SubscribeRequest : InternalPacket
    {
        public string Name;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Name = deserializer.Read(Name);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Name);
        }
    }
}