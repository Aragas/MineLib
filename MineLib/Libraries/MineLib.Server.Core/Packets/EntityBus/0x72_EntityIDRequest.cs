using Aragas.Network.Attributes;
using Aragas.QServer.Core.Packets;

namespace MineLib.Server.Core.Packets.EntityBus
{
    [Packet(0x72)]
    public sealed class EntityIDRequest : InternalPacket { }
}