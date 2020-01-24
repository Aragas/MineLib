using App.Metrics;
using App.Metrics.Counter;

using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;
using Aragas.QServer.Core;
using Aragas.QServer.Core.BackgroundServices;

using Microsoft.Extensions.Logging;

using MineLib.Protocol.Packets;
using MineLib.Server.Proxy.Data;
using MineLib.Server.Proxy.Protocol.Netty;

using System;

namespace MineLib.Server.Proxy.BackgroundServices
{
    internal abstract class ProxyListenerService<TConnection, TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer> :
        ListenerService<TConnection, TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer>
        where TConnection : DefaultConnectionHandler<TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer>
        where TPacketTransmission : SocketPacketTransmission<TPacket, TIDType, TSerializer, TDeserializer>
        where TPacket : Packet<TIDType>
        where TSerializer : StreamSerializer, new()
        where TDeserializer : StreamDeserializer, new()
    {
        public ServerInfo ServerInfo { get; }

        internal ProxyListenerService(ServerInfo serverInfo, IServiceProvider serviceProvider, ILogger<ProxyNettyListenerService> logger) : base(serviceProvider, logger)
        {
            ServerInfo = serverInfo;
        }

        protected override void OnClientConnected(TConnection client)
        {
            ServerInfo.CurrentConnections--;

            base.OnClientConnected(client);
        }
        protected override void OnClientDisconnected(object sender, EventArgs e)
        {
            ServerInfo.CurrentConnections--;

            base.OnClientDisconnected(sender, e);
        }
    }

    internal sealed class ProxyLegacyListenerService :
        ProxyListenerService<PlayerNettyConnection, ProxyNettyTransmission, MinecraftPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public override int Port { get; } = 25565;
        private IMetrics Metrics { get; }
        private CounterOptions PlayersConnectedCounter { get; } = new CounterOptions()
        {
            Name = "Legacy Players Connected",
            MeasurementUnit = Unit.Connections
        };

        public ProxyLegacyListenerService(IMetrics metrics, ServerInfo serverInfo, IServiceProvider serviceProvider, ILogger<ProxyNettyListenerService> logger) : base(serverInfo, serviceProvider, logger)
        {
            Metrics = metrics;
            Metrics.Measure.Counter.Increment(PlayersConnectedCounter);
            Metrics.Measure.Counter.Decrement(PlayersConnectedCounter);
        }

        protected override void OnClientConnected(PlayerNettyConnection client)
        {
            Metrics.Measure.Counter.Increment(PlayersConnectedCounter);

            base.OnClientConnected(client);
        }
        protected override void OnClientDisconnected(object sender, EventArgs e)
        {
            Metrics.Measure.Counter.Decrement(PlayersConnectedCounter);

            base.OnClientDisconnected(sender, e);
        }
    }
}