/*
using App.Metrics;
using App.Metrics.Counter;

using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core;

using MineLib.Server.Proxy.Packets.Netty;
using MineLib.Server.Proxy.Protocol.Factory;
using MineLib.Server.Proxy.Protocol.Netty;

using System;

namespace MineLib.Server.Proxy
{
    internal sealed class PlayerNettyListener : DefaultListener<PlayerNettyConnection, EmptyFactory, ProxyNettyTransmission, ProxyNettyPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public override int Port { get; } = 25565;

        private IMeasureMetrics Measure { get; }
        private CounterOptions PlayersConnectedCounter { get; } = new CounterOptions()
        {
            Name = "Players Connected (Netty Protocols)",
            MeasurementUnit = Unit.Connections
        };

        public PlayerNettyListener(IMeasureMetrics measure)
        {
            Measure = measure;
            Measure.Counter.Increment(PlayersConnectedCounter);
            Measure.Counter.Decrement(PlayersConnectedCounter);
        }

        protected override void OnClientConnected(PlayerNettyConnection client)
        {
            Measure.Counter.Increment(PlayersConnectedCounter);

            base.OnClientConnected(client);
        }

        protected override void OnClientDisconnected(object sender, EventArgs e)
        {
            Measure.Counter.Decrement(PlayersConnectedCounter);

            base.OnClientDisconnected(sender, e);
        }
    }
}
*/