using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Core.Packets.MBus
{
    [Packet(0x11)]
    public sealed class Message : InternalPacket
    {
        public byte[] Data;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Data = deserializer.Read(Data);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Data);
        }
    }
}