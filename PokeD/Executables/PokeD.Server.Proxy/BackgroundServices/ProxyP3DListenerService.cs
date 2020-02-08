using App.Metrics;
using App.Metrics.Counter;

using Aragas.QServer.Core.BackgroundServices;

using Microsoft.Extensions.Logging;
using PokeD.Core.IO;

using PokeD.Core.Packets.P3D;

using PokeD.Server.Proxy.Protocol.P3D;
using System;

namespace PokeD.Server.Proxy
{
    internal sealed class ProxyP3DListenerService :
        ListenerService<ProxyP3DConnection, ProxyP3DTransmission, P3DPacket, int, P3DSerializer, P3DDeserializer>
    {
        public override int Port { get; } = 15124;
        private IMetrics Metrics { get; }
        private CounterOptions PlayersConnectedCounter { get; } = new CounterOptions()
        {
            Name = "P3D Players Connected",
            MeasurementUnit = Unit.Connections
        };

        public ProxyP3DListenerService(IMetrics metrics, IServiceProvider serviceProvider, ILogger<ProxyP3DListenerService> logger) : base(serviceProvider, logger)
        {
            Metrics = metrics;
            Metrics.Measure.Counter.Increment(PlayersConnectedCounter);
            Metrics.Measure.Counter.Decrement(PlayersConnectedCounter);
        }

        protected override void OnClientConnected(ProxyP3DConnection client)
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