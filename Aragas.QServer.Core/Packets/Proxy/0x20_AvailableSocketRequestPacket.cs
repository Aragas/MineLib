using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace Aragas.QServer.Core.Packets.PlayerHandler
{
    [Packet(0x20)]
    public sealed class AvailableSocketRequestPacket : InternalPacket
    {
        public VarInt ProtocolVersion;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            ProtocolVersion = deserializer.Read(ProtocolVersion);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(ProtocolVersion);
        }
    }
}