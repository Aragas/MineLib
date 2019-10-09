using Aragas.Network.Attributes;

namespace MineLib.Server.Core.Packets
{
    [Packet(0x00)]
    public sealed class PingPacket : InternalPacket { }
}