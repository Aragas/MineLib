using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace Aragas.QServer.Core.Packets.PlayerHandler
{
    [Packet(0x23)]
    public sealed class CreateProxyResponsePacket : InternalPacket
    {
        public VarInt Port;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Port = deserializer.Read(Port);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Port);
        }
    }
}