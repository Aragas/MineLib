﻿using App.Metrics;
using App.Metrics.Counter;

using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core.BackgroundServices;

using Microsoft.Extensions.Logging;

using MineLib.Protocol.Packets;
using MineLib.Server.Proxy.Protocol.Netty;

using System;

namespace MineLib.Server.Proxy.BackgroundServices
{
    internal sealed class ProxyNettyListenerService :
        ListenerService<PlayerNettyConnection, ProxyNettyTransmission, MinecraftPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public override int Port { get; } = 25565;
        private IMetrics Metrics { get; }
        private CounterOptions PlayersConnectedCounter { get; } = new CounterOptions()
        {
            Name = "Netty Players Connected",
            MeasurementUnit = Unit.Connections
        };

        public ProxyNettyListenerService(IMetrics metrics, IServiceProvider serviceProvider, ILogger<ProxyNettyListenerService> logger) : base(serviceProvider, logger)
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