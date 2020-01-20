using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Netty.Packets.Server;
using MineLib.Server.Proxy.Data;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets
{
    [Packet(0xFF)]
    internal sealed class LegacyDisconnectPacket : ServerStatusPacket
    {
        public UTF16BEString Response { get; set; } = default!;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Response = deserializer.Read(Response);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Response);
        }
    }
}