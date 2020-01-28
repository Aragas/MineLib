using App.Metrics;
using App.Metrics.Counter;

using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core.BackgroundServices;

using Microsoft.Extensions.Logging;

using MineLib.Protocol.Packets;
using MineLib.Server.Proxy.Protocol.Netty;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.Proxy.BackgroundServices
{
    internal sealed class ProxyClassicListenerService :
        ListenerService<PlayerNettyConnection, ProxyNettyTransmission, MinecraftPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public override int Port { get; } = 56552;
        private IMetrics Metrics { get; }
        private CounterOptions PlayersConnectedCounter { get; } = new CounterOptions()
        {
            Name = "Classic Players Connected",
            MeasurementUnit = Unit.Connections
        };

        private Task _hearthbeat;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();


        public ProxyClassicListenerService(IMetrics metrics, IServiceProvider serviceProvider, ILogger<ProxyNettyListenerService> logger) : base(serviceProvider, logger)
        {
            Metrics = metrics;
            Metrics.Measure.Counter.Increment(PlayersConnectedCounter);
            Metrics.Measure.Counter.Decrement(PlayersConnectedCounter);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _hearthbeat = HearthbeatAsync(_stoppingCts.Token);

            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_hearthbeat != null)
            {
                try
                {
                    // Signal cancellation to the executing method
                    _stoppingCts.Cancel();
                }
                finally
                {
                    // Wait until the task completes or the stop token triggers
                    await Task.WhenAny(_hearthbeat, Task.Delay(Timeout.Infinite, cancellationToken));
                }
            }

            await base.StopAsync(cancellationToken);
        }


        /// <summary> Server's public URL, as returned by the heartbeat server.
        /// This is the URL that players should be able to connect by.
        /// May be null (if heartbeat is disabled, or first heartbeat has not been sent yet). </summary>
        public Uri Url { get; internal set; }

        internal Uri HeartbeatServerUrl;
        readonly TimeSpan DelayDefault = TimeSpan.FromSeconds(20);
        readonly TimeSpan TimeoutDefault = TimeSpan.FromSeconds(10);

        /// <summary> Delay between sending heartbeats. Default: 20s </summary>
        public TimeSpan Delay { get; set; }

        /// <summary> Request timeout for heartbeats. Default: 10s </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary> Secret string used to verify players' names.
        /// Randomly generated at startup, and can be randomized by "/reload salt"
        /// Known only to this server and to heartbeat server(s). </summary>
        public string Salt { get; internal set; }
        protected async Task HearthbeatAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _timeout = 45000; // Beat every 30 seconds
                serverURL = "http://www.minecraft.net/heartbeat.jsp";
                staticPostVars = "port=" + Properties.ServerPort +
                                 "&max=" + Properties.MaxPlayers +
                                 "&name=" + Uri.EscapeDataString(Properties.ServerName) +
                                 "&public=" + Properties.PublicServer +
                                 "&version=7";

                await Task.Delay(_timeout);
            }
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