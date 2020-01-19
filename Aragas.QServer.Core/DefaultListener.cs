﻿using Aragas.Network.IO;
using Aragas.Network.Packets;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Aragas.QServer.Core
{
    public abstract class DefaultListener<TConnection, TFactory, TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer> : BaseListener
        where TConnection : DefaultConnectionHandler<TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer>, new()
        where TFactory : BasePacketFactory<TPacket, TIDType>, new()
        where TPacketTransmission : SocketPacketTransmission<TPacket, TIDType, TSerializer, TDeserializer>, new()
        where TPacket : Packet<TIDType>
        where TSerializer : StreamSerializer, new()
        where TDeserializer : StreamDeserializer, new()
    {
        protected List<TConnection> Connections { get; } = new List<TConnection>();

        public sealed override void Start()
        {
#if IPV6
            Listener = new TcpListener(new IPEndPoint(IPAddress.IPv6Any, Port));
            Listener.Server.DualMode = true;
#else
            Listener = new TcpListener(new IPEndPoint(IPAddress.Any, Port));
#endif
            Listener.Server.ReceiveTimeout = 5000;
            Listener.Server.SendTimeout = 5000;
            Listener.Start();
            Console.WriteLine($"Started {GetType().Name} on port {Port}.");

            new Thread(ListenerCycle)
            {
                Name = $"{GetType().Name}Thread",
                IsBackground = true
            }.Start();
            Console.WriteLine($"Started {GetType().Name}Thread.");
        }
        public sealed override void Stop()
        {
            Listener?.Stop();
            Console.WriteLine($"Stopped {GetType().Name}");

            lock (Connections)
            {
                foreach (var client in Connections)
                    client?.Dispose();
                Connections.Clear();
            }
        }

        protected virtual void OnClientConnected(TConnection client)
        {
            client.StartListening();
            client.Disconnected += (this, OnClientDisconnected);

            lock (Connections)
                Connections.Add(client);

#if DEBUG
            Console.WriteLine($"{client.GetType().Name} connected.");
#endif
        }

        protected virtual void OnClientDisconnected(object sender, EventArgs e)
        {
            if (sender is TConnection client)
            {
#if DEBUG
                Console.WriteLine($"{client.GetType().Name} disconnected.");
#endif

                client.Disconnected -= OnClientDisconnected;
                lock (Connections)
                    Connections.Remove(client);
                client.Dispose();
            }
        }

        private void ListenerCycle()
        {
            try
            {
                while (Listener != null) // Listener.Stop() will stop it.
                {
                    var client = new TConnection()
                    {
                        Stream = new TPacketTransmission()
                        {
                            Socket = Listener.AcceptSocket(),
                            Factory = new TFactory()
                        }
                    };
                    OnClientConnected(client);
                }

            }
            catch (Exception e) when (e is SocketException) { /* ignore */ }
        }
    }
}