using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Protocol.Netty.Packets;
using MineLib.Protocol.Netty.Packets.Server;

namespace MineLib.Protocol.Netty.Protocol
{
    public sealed class ServerHandshakeEnumFactory : ProtocolNettyFactory<ProtocolNettyPacket<ServerHandshakePacketTypes>, ServerHandshakePacketTypes> { }

    public sealed class ServerHandshakeFactory : DefaultPacketFactory<ServerHandshakePacket, VarInt, ProtobufSerializer, ProtobufDeserializer>  { }
}