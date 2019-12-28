using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Server.Proxy.Packets.Netty;

namespace MineLib.Server.Proxy.Protocol.Factory.Netty
{
    internal sealed class HandshakeStateFactory : DefaultPacketFactory<HandshakeStatePacket, VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}