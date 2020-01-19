using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace Aragas.QServer.Core.Packets.PlayerHandler
{
    [Packet(0x22)]
    public sealed class CreateProxyRequestPacket : InternalPacket
    {
        public VarInt ProtocolVersion;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            ProtocolVersion = deserializer.Read(ProtocolVersion);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(ProtocolVersion);
        }
    }
}