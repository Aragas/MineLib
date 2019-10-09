using Aragas.Network.Attributes;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x31)]
    public sealed class WorldInfoResponse : InternalPacket { }
}