using Aragas.Network.Data;

using MineLib.Server.Core;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MineLib.Server.PlayerBus
{
    internal sealed class ProxyConnectionHandler
    {
        public static Dictionary<VarInt, ProxyConnectionHandler> Protocols { get; } = new Dictionary<VarInt, ProxyConnectionHandler>();
        public static ProxyConnectionHandler GetProxyConnectionHandler(VarInt protocolVersion)
        {
            if (!Protocols.ContainsKey(protocolVersion))
                Protocols.Add(protocolVersion, new ProxyConnectionHandler(protocolVersion));
            return Protocols[protocolVersion];
        }

        public int ProtocolVersion { get; }
        public int Port { get; } = DefaultValues.Proxy_Port;
        private TcpListener? Listener { get; set; }

        private List<PlayerHandler.PlayerHandler> Clients { get; } = new List<PlayerHandler.PlayerHandler>();

        public ProxyConnectionHandler(int protocolVersion)
        {
            ProtocolVersion = protocolVersion;
            Port += ProtocolVersion;
        }

        public void Start()
        {
            Console.WriteLine($"Starting Listener");

            Listener = new TcpListener(new IPEndPoint(IPAddress.IPv6Any, Port));
            Listener.Server.DualMode = true;
            Listener.Server.ReceiveTimeout = 5000;
            Listener.Server.SendTimeout = 5000;
            Listener.Start();

            new Thread(ListenerCycle)
            {
                Name = "ProxyConnectionHandlerListenerThread",
                IsBackground = true
            }.Start();
        }
        public void Stop()
        {
            Console.WriteLine($"Stopping Listener");

            Listener?.Stop();

            lock (Clients)
            {
                foreach (var client in Clients)
                    client?.Dispose();
                Clients.Clear();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private void ListenerCycle()
        {
            try
            {
                while (Listener != null) // Listener.Stop() will stop it.
                {
                    var client = new PlayerHandler.PlayerHandler(Listener.AcceptSocket(), ProtocolVersion);
                    Console.WriteLine($"Proxy connected. Protocol version {ProtocolVersion}");

                    lock (Clients)
                        Clients.Add(client);
                }
            }
            catch (SocketException) { }
        }
    }
}