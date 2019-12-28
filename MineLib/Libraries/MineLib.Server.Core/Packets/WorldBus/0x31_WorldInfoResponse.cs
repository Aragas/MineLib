using Aragas.Network.Attributes;
using Aragas.QServer.Core.Packets;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x31)]
    public sealed class WorldInfoResponse : InternalPacket { }
}