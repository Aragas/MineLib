using Aragas.Network.Attributes;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x30)]
    public sealed class WorldInfoRequest : InternalPacket { }
}