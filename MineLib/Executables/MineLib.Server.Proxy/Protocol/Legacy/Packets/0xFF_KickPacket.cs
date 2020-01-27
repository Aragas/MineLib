using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Proxy.Protocol.Legacy.Packets
{
    [PacketID(0xFF)]
    internal sealed class KickPacket : ProxyLegacyPacket
    {
        public string Message { get; set; } = default!;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Message = deserializer.Read(Message);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Message);
        }
    }
}