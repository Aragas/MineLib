using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace Aragas.QServer.Core.Packets.PlayerHandler
{
    [PacketID(0x23)]
    public sealed class CreateProxyResponsePacket : InternalPacket
    {
        public VarInt Port;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Port = deserializer.Read(Port);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Port);
        }
    }
}