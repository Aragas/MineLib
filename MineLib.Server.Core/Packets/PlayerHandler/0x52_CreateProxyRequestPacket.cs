using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Server.Core.Packets.PlayerHandler
{
    [Packet(0x52)]
    public sealed class CreateProxyRequestPacket : InternalPacket
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