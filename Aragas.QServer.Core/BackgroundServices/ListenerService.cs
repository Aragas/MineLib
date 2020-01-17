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
    public abstract class ListenerService<TConnection, TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer> : BackgroundService
        where TConnection : DefaultConnectionHandler<TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer>
        where TPacketTransmission : SocketPacketTransmission<TPacket, TIDType, TSerializer, TDeserializer>
        where TPacket : Packet<TIDType, TSerializer, TDeserializer>
        where TSerializer : StreamSerializer, new()
        where TDeserializer : StreamDeserializer, new()
    {
        private static bool IPv4 { get; } =
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is string str && str.Equals("true", StringComparison.OrdinalIgnoreCase);

        public abstract int Port { get; }
        protected TcpListener Listener { get; }
        protected ConcurrentDictionary<TConnection, object?> Connections { get; } = new ConcurrentDictionary<TConnection, object?>();
        protected IServiceProvider ServiceProvider { get; }
        protected ILogger Logger { get; }
        protected ObjectFactory ClientFactory { get; } = ActivatorUtilities.CreateFactory(typeof(TConnection), new[] { typeof(Socket) });

        protected ListenerService(IServiceProvider serviceProvider, ILogger logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;

            if (IPv4)
            {
                Listener = new TcpListener(new IPEndPoint(IPAddress.Any, Port));
            }
            else
            {
                Listener = new TcpListener(new IPEndPoint(IPAddress.IPv6Any, Port));
                Listener.Server.DualMode = true;
            }
            Listener.Server.NoDelay = true;
            Listener.Server.ReceiveTimeout = 5000;
            Listener.Server.SendTimeout = 5000;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Listener.Start();
            stoppingToken.Register(() => Listener.Stop());

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var socket = await Listener.AcceptSocketAsync();
                    socket.NoDelay = true;
                    var client = (TConnection) ClientFactory(ServiceProvider, new[] { socket });
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