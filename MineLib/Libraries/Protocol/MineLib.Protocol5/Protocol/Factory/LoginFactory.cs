using MineLib.Protocol5.Packets;

namespace MineLib.Protocol5.Protocol
{
    public sealed class LoginFactory : Protocol5Factory<ServerLoginPacket, ServerLoginPacketTypes> { }
}