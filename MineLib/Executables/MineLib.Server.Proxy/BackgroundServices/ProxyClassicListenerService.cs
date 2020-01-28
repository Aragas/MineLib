using App.Metrics;
using App.Metrics.Counter;

using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core.BackgroundServices;

using Microsoft.Extensions.DependencyInjection;
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

        private Task? _hearthbeat;
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
                    await Task.WhenAny(_hearthbeat, Task.Delay(System.Threading.Timeout.Infinite, cancellationToken));
                }
            }

            await base.StopAsync(cancellationToken);
        }

        private async Task HearthbeatAsync(CancellationToken stoppingToken)
        {
            var heartbeat = ActivatorUtilities.CreateInstance<Heartbeat>(ServiceProvider, new object[] { BeatType.Minecraft, (ushort) Port });
            heartbeat.Beat(true);
            while (!stoppingToken.IsCancellationRequested)
            {
                heartbeat.Beat(false);

                await Task.Delay(45000);
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