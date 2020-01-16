﻿using App.Metrics;
using App.Metrics.Counter;

using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core.BackgroundServices;

using Microsoft.Extensions.Logging;

using MineLib.Server.Proxy.Protocol.Netty;
using MineLib.Server.Proxy.Protocol.Netty.Packets;

using System;

namespace MineLib.Server.Proxy.BackgroundServices
{
    internal sealed class ProxyClassicListenerService :
        ListenerService<PlayerNettyConnection, ProxyNettyTransmission, ProxyNettyPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public override int Port { get; } = 56552;
        private IMetrics Metrics { get; }
        private CounterOptions PlayersConnectedCounter { get; } = new CounterOptions()
        {
            Name = "Classic Players Connected",
            MeasurementUnit = Unit.Connections
        };

        public ProxyClassicListenerService(IMetrics metrics, IServiceProvider serviceProvider, ILogger<ProxyNettyListenerService> logger) : base(serviceProvider, logger)
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