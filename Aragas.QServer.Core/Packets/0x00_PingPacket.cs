using Aragas.Network.Attributes;

namespace Aragas.QServer.Core.Packets
{
    [Packet(0x00)]
    public sealed class PingPacket : InternalPacket { }
}