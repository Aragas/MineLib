using App.Metrics;

using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;
using Aragas.QServer.Core;
using Aragas.QServer.Prometheus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MineLib.Server.Proxy.Packets.Netty;
using MineLib.Server.Proxy.Protocol.Factory;
using MineLib.Server.Proxy.Protocol.Netty;

using Serilog;
using Serilog.Events;

using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MineLib.Server.Proxy
{
    internal sealed class ProxyNettyListenerService : ListenerService<PlayerNettyConnection, EmptyFactory, ProxyNettyTransmission, ProxyNettyPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public ProxyNettyListenerService(ILogger<ProxyNettyListenerService> logger) : base(logger) { }

        public override int Port { get; } = 25565;
    }

    public abstract class ListenerService<TConnection, TFactory, TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer> : BackgroundService
        where TConnection : DefaultConnectionHandler<TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer>, new()
        where TFactory : BasePacketFactory<TPacket, TIDType, TSerializer, TDeserializer>, new()
        where TPacketTransmission : SocketPacketTransmission<TPacket, TIDType, TSerializer, TDeserializer>, new()
        where TPacket : Packet<TIDType, TSerializer, TDeserializer>
        where TSerializer : StreamSerializer, new()
        where TDeserializer : StreamDeserializer, new()
    {
        private static bool IPv4 { get; } =
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is string str && str.Equals("true", StringComparison.OrdinalIgnoreCase);

        public abstract int Port { get; }
        protected TcpListener? Listener { get; set; }
        protected ConcurrentDictionary<TConnection, object?> Connections { get; } = new ConcurrentDictionary<TConnection, object?>();
        protected ILogger Logger { get; }

        protected ListenerService(ILogger logger)
        {
            Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (IPv4)
            {
                Listener = new TcpListener(new IPEndPoint(IPAddress.Any, Port));
            }
            else
            {
                Listener = new TcpListener(new IPEndPoint(IPAddress.IPv6Any, Port));
                Listener.Server.DualMode = true;
            }
            Listener.Server.ReceiveTimeout = 5000;
            Listener.Server.SendTimeout = 5000;
            Logger.LogInformation("{TypeName}: Starting Listener.", GetType().Name);
            Listener.Start();


            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var client = new TConnection()
                    {
                        Stream = new TPacketTransmission()
                        {
                            Socket = await Listener.AcceptSocketAsync(),
                            Factory = new TFactory()
                        }
                    };
                    OnClientConnected(client);
                }
                catch (Exception ex) when (ex is SocketException)
                {
                    Logger.LogWarning(ex, "{TypeName}: SocketException.", GetType().Name);
                }
            }


            Logger.LogInformation("{TypeName}: Stopping Listener.", GetType().Name);
            Listener?.Stop();
        }

        protected virtual void OnClientConnected(TConnection client)
        {
            client.StartListening();
            client.Disconnected += (this, OnClientDisconnected);

            Connections.TryAdd(client, null);

            Logger.LogInformation("{ClientTypeName}: Connected.", client.GetType().Name);
        }
        protected virtual void OnClientDisconnected(object sender, EventArgs e)
        {
            if (sender is TConnection client)
            {
                Logger.LogInformation("{ClientTypeName}: Disconnected.", client.GetType().Name);

                client.Disconnected -= OnClientDisconnected;
                lock (Connections)
                    Connections.TryRemove(client, out _);
                client.Dispose();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var keyValue in Connections)
                keyValue.Key?.Dispose();
            Connections?.Clear();
        }
    }

    public abstract class BaseHostProgram
    {
        public static async Task Main<TProgram>(string[] args) where TProgram : BaseHostProgram, new()
        {
            Aragas.Network.Extensions.PacketExtensions.Init();
            //MineLib.Core.Extensions.PacketExtensions.Init();
            //MineLib.Server.Core.Extensions.PacketExtensions.Init();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("{TypeName}: Starting.", typeof(TProgram).Name);
                var program = new TProgram();
                await program.CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{TypeName}: Fatal exception.", typeof(TProgram).Name);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public abstract IHostBuilder CreateHostBuilder(string[] args);
    }

    public class HostProgram : BaseHostProgram
    {
        public static async Task Main(string[] args) => await Main<HostProgram>(args);

        public override IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMetrics(metricsBuilder =>
                {
                    metricsBuilder
                        .OutputMetrics.AsPrometheusPlainText();

                });
                services.AddMetricsReportingHostedService();

                services.AddHostedService<CpuUsageMetricsService>();
                services.AddHostedService<MemoryUsageMetricsService>();
                services.AddHostedService<StandardMetricsService>();

                services.AddHostedService<ProxyNettyListenerService>();
            })
            .UseSerilog();
    }
}