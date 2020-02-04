using Aragas.Network.Packets;

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Aragas.Network.IO
{
    public abstract class SocketPacketTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer> : PacketStreamTransmission<TPacketType, TPacketIDType>
        where TPacketType : Packet<TPacketIDType>
        where TSerializer : StreamSerializer, new()
        where TDeserializer : StreamDeserializer, new()
    {
        protected Socket? _socket;
        public virtual Socket Socket
        {
            get => _socket!; // Because we can't create a Generic with new() with properties
            set
            {
                if (_socket != null)
                    throw new InvalidOperationException("You can't set Socket once it's initialized");
                _socket = value;
                Stream = new SocketClientStream(Socket);
                //Stream = new NetworkStream(Socket);
            }
        }

        public virtual string Host => (Socket.RemoteEndPoint as IPEndPoint)?.Address.ToString() ?? string.Empty;
        public virtual ushort Port => (ushort) ((Socket.RemoteEndPoint as IPEndPoint)?.Port ?? 0);
        public virtual bool IsConnected => Socket?.Connected == true;
        public override long AvailableData => Socket?.Available ?? -1;

        public BasePacketFactory<TPacketType, TPacketIDType>? DefaultFactory { get; set; }

        protected SocketPacketTransmission() : base() { }
        protected SocketPacketTransmission(Socket socket, Stream? socketStream = null, BasePacketFactory<TPacketType, TPacketIDType>? factory = null)
            : base(socketStream ?? new SocketClientStream(socket))
        {
            Socket = socket;
            DefaultFactory = factory;
        }

        public virtual void Connect(string ip, ushort port) { Socket.Connect(ip, port); }
        public virtual void Disconnect() { Socket.Disconnect(false); }

        protected virtual void Send(in ReadOnlySpan<byte> span) => Stream.Write(span);
        protected virtual Span<byte> Receive(long length)
        {
            Span<byte> buffer = new byte[length];
            int received = 0;
            while (received < length)
                received += Stream.Read(buffer);
            return buffer;
        }

        public override void SendPacket(TPacketType packet)
        {
            using var serializer = new TSerializer();
            serializer.Write(packet.ID);
            packet.Serialize(serializer);
            Send(serializer.GetData());
        }

        public virtual bool TryReadPacket(out TPacketType? packet)
        {
            packet = ReadPacket();
            return packet != null;
        }
        public override TPacketType? ReadPacket()
        {
            if (DefaultFactory == null)
                throw new NullReferenceException($"Property {nameof(DefaultFactory)} is null. Provide an implementation or override {nameof(ReadPacket)} with your implementation.");

            if (Socket.Available > 0)
            {
                using var deserializer = StreamDeserializer.Create<TDeserializer>(Stream);
                var id = deserializer.Read<TPacketIDType>();
                var packet = DefaultFactory.Create(id);
                if (packet != null)
                {
                    packet.Deserialize(deserializer);
                    return packet;
                }
            }

            return null;
        }
    }
}