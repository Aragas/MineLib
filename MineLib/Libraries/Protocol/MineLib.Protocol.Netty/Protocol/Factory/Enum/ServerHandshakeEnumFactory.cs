using MineLib.Protocol.Netty.Packets;
using MineLib.Protocol.Netty.Packets.Enum;

namespace MineLib.Protocol.Netty.Protocol
{
    public sealed class ServerHandshakeEnumFactory : ProtocolNettyFactory<ProtocolNettyPacket<ServerHandshakePacketTypes>, ServerHandshakePacketTypes> { }
}