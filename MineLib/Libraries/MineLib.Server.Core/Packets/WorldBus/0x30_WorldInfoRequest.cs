using Aragas.Network.Attributes;
using Aragas.QServer.Core.Packets;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [PacketID(0x30)]
    public sealed class WorldInfoRequest : InternalPacket { }
}