using Aragas.Network.IO;
using Aragas.Network.Packets;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.BackgroundServices
{
    /*
    public interface ClientFactory<TConnection, TFactory, TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer>
        where TConnection : DefaultConnectionHandler<TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer>, new()
        where TFactory : BasePacketFactory<TPacket, TIDType, TSerializer, TDeserializer>, new()
        where TPacketTransmission : SocketPacketTransmission<TPacket, TIDType, TSerializer, TDeserializer>, new()
        where TPacket : Packet<TIDType, TSerializer, TDeserializer>
        where TSerializer : StreamSerializer, new()
        where TDeserializer : StreamDeserializer, new()
    {
        TConnection GetClient(Socket socket)
        {
            return new TConnection()
            {
                Stream = new TPacketTransmission()
                {
                    Socket = socket,
                    Factory = new TFactory()
                }
            };
        }
    }
    */

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
        protected TcpListener Listener { get; set; } = default!;
        protected ConcurrentDictionary<TConnection, object?> Connections { get; } = new ConcurrentDictionary<TConnection, object?>();
        protected ILogger Logger { get; }

        protected ListenerService(ILogger logger)
        {
            Logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
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

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => Listener.Stop());

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //IServiceProvider sp = null;
                    //var socket = await Listener.AcceptSocketAsync();
                    //
                    //var client = sp.GetRequiredService<TConnection>();

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
}