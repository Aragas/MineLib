using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Netty.Packets.Server;
using MineLib.Server.Proxy.Data;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets
{
    [Packet(0xFF)]
    internal sealed class LegacyDisconnectPacket : ServerStatusPacket
    {
        public UTF16BEString Response { get; set; }

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
            Response = deserialiser.Read(Response);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Response);
        }
    }
}