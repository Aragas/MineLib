using MineLib.Protocol.Netty.Packets;

namespace MineLib.Protocol.Netty.Protocol
{
    public sealed class HandshakeFactory : ProtocolNettyFactory<ServerHandshakePacket, ServerHandshakePacketTypes> { }
}