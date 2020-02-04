using Aragas.TupleEventSystem;
using Aragas.QServer.Core.Packets;
using Aragas.QServer.Core.Packets.MBus;
using Aragas.QServer.Core.Protocol;

using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.MBus
{
    // mbus://<host>:<port>/<name>
    public sealed class TCPNetworkMBus : InternalConnectionHandler, IMBus
    {
        private static bool InContainer => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is string str && str == "true";

        private static ushort DefaultPort = 0x5000;
        public static (string Host, ushort Port, string Name) ParseURI(ReadOnlySpan<char> span)
        {
            if (!span.StartsWith("mbus://".AsSpan()))
                return default;
            span = span.Slice(7);

            if (span.IsEmpty)
                return default;

            ReadOnlySpan<char> host;
            ReadOnlySpan<char> port;
            ReadOnlySpan<char> name;
            var hostDelimiterIndex = span.IndexOf('/');
            if (hostDelimiterIndex == -1)
            {
                return default;
            }
            else
            {
                host = span.Slice(0, hostDelimiterIndex);
                var portDelimiterIndex = span.IndexOf(':');
                if (portDelimiterIndex == -1)
                {
                    port = DefaultPort.ToString().AsSpan();
                }
                else
                {
                    host = span.Slice(0, portDelimiterIndex);
                    port = span.Slice(portDelimiterIndex + 1, hostDelimiterIndex - portDelimiterIndex - 1);
                }

                name = span.Slice(hostDelimiterIndex + 1);
            }


            return (host.ToString(), ushort.TryParse(port.ToString(), out var parsedPort) ? parsedPort : DefaultPort, name.ToString());
        }
        private static Socket Connect(string host, ushort port)
        {
            var socket = InContainer
                ? new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp) { DualMode = true }
                : new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(host, port);
            return socket;
        }


        public static readonly TimeSpan DefaultMessageTTL = TimeSpan.FromMilliseconds(500);

        //public event EventHandler<MBusMessageReceivedEventArgs> MessageReceived;
        public BaseEventHandler<MBusMessageReceivedEventArgs> MessageReceived { get; set; } = new WeakReferenceEventHandler<MBusMessageReceivedEventArgs>();

        public string Name { get; }
        public TimeSpan MessageTTL { get; }

        public TCPNetworkMBus(string uri) : this(ParseURI(uri.AsSpan()), DefaultMessageTTL) { }
        public TCPNetworkMBus(string uri, TimeSpan messageTTL) : this(ParseURI(uri.AsSpan()), messageTTL) { }
        private TCPNetworkMBus((string Host, ushort Port, string Name) parsedURI, TimeSpan messageTTL) : base(Connect(parsedURI.Host, parsedURI.Port))
        {
            Name = parsedURI.Name;
            MessageTTL = messageTTL;

            StartListening();
            SendPacket(new SubscribeRequest() { Name = Name });
        }

        public void SendMessage(in ReadOnlySpan<byte> message) =>
            SendPacket(new Message() { Data = message.ToArray() });
        public Task SendMessageAsync(ReadOnlyMemory<byte> message) => Task.Run(() =>
            SendPacket(new Message() { Data = message.ToArray() }));

        protected override void HandlePacket(InternalPacket packet)
        {
            switch (packet)
            {
                case PingPacket pingPacket:
                    SendPacket(new PingPacket() { GUID = pingPacket.GUID });
                    break;

                case Message message:
                    MessageReceived?.Invoke(this, new MBusMessageReceivedEventArgs(message.Data));
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                MessageReceived?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}