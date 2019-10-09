using Aragas.Network.Attributes;

namespace MineLib.Server.Core.Packets.EntityBus
{
    [Packet(0x72)]
    public sealed class EntityIDRequest : InternalPacket { }
}