using Aragas.TupleEventSystem;
using MineLib.Server.Core.Packets;
using MineLib.Server.Core.Packets.MBus;
using MineLib.Server.Core.Protocol;

using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MineLib.Server.Core
{
    // mbus://<host>:<port>/<name>
    public sealed class NetworkMBus : InternalConnectionHandler, IMBus
    {
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
            var hostDelimeterIndex = span.IndexOf('/');
            if (hostDelimeterIndex == -1)
            {
                return default;
            }
            else
            {
                host = span.Slice(0, hostDelimeterIndex);
                var portDelimeterIndex = span.IndexOf(':');
                if (portDelimeterIndex == -1)
                {
                    port = DefaultValues.MBus_Port.ToString().AsSpan();
                }
                else
                {
                    host = span.Slice(0, portDelimeterIndex);
                    port = span.Slice(portDelimeterIndex + 1, hostDelimeterIndex - portDelimeterIndex - 1);
                }

                name = span.Slice(hostDelimeterIndex + 1);
            }


            return (host.ToString(), ushort.TryParse(port.ToString(), out var parsedPort) ? parsedPort : DefaultValues.MBus_Port, name.ToString());
        }
        private static Socket Connect(string host, ushort port)
        {
            var client = new TcpClient();
            //client.Client.DualMode = true;
            client.Connect(host, port);
            return client.Client;
        }


        public static readonly TimeSpan DefaultMessageTTL = TimeSpan.FromMilliseconds(500);

        //public event EventHandler<MBusMessageReceivedEventArgs> MessageReceived;
        public BaseEventHandler<MBusMessageReceivedEventArgs> MessageReceived { get; set; } = new WeakReferenceEventHandler<MBusMessageReceivedEventArgs>();

        public string Name { get; }
        public TimeSpan MessageTTL { get; }

        public NetworkMBus(string uri) : this(ParseURI(uri.AsSpan()), DefaultMessageTTL) { }
        public NetworkMBus(string uri, TimeSpan messageTTL) : this(ParseURI(uri.AsSpan()), messageTTL) { }
        private NetworkMBus((string Host, ushort Port, string Name) parsedURI, TimeSpan messageTTL) : base(Connect(parsedURI.Host, parsedURI.Port))
        {
            Name = parsedURI.Name;
            MessageTTL = messageTTL;

            StartListening();
            SendPacket(new SubscribeRequest() { Name = Name });
        }

        public void SendMessage(in ReadOnlySpan<byte> message) => SendPacket(new Message() { Data = message.ToArray() });
        public Task SendMessageAsync(ReadOnlyMemory<byte> message) => Task.Factory.StartNew(() => SendMessage(message.Span));

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