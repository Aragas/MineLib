using Aragas.Network.Attributes;

namespace MineLib.Server.Core.Packets.EntityBus
{
    [Packet(0x70)]
    public sealed class PlayerListRequest : InternalPacket { }
}