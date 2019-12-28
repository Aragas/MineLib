using Aragas.Network.Attributes;
using Aragas.QServer.Core.Packets;

namespace MineLib.Server.Core.Packets.EntityBus
{
    [Packet(0x70)]
    public sealed class PlayerListRequest : InternalPacket { }
}