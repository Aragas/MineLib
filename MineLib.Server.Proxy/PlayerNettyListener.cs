using Aragas.Network.Data;
using Aragas.Network.IO;

using MineLib.Server.Core;
using MineLib.Server.Proxy.Packets.Netty;
using MineLib.Server.Proxy.Protocol.Factory;
using MineLib.Server.Proxy.Protocol.Netty;

namespace MineLib.Server.Proxy
{
    internal sealed class PlayerNettyListener : DefaultListener<PlayerNettyConnection, EmptyFactory, ProxyNettyTransmission, ProxyNettyPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public override int Port { get; } = DefaultValues.Proxy_Netty_Port;
    }
}