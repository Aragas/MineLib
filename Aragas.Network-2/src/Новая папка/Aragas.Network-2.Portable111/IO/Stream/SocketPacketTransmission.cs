using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Aragas.Network.Data;
using Aragas.Network.Packets;

namespace Aragas.Network.IO
{
    public class SocketPacketTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer> : PacketStreamTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer>
        where TPacketType : Packet<TPacketIDType, TSerializer, TDeserializer>
        where TSerializer : StreamSerializer, new()
        where TDeserializer : StreamDeserializer, new()
    {
        public static TPacketTransmission Create<TPacketTransmission>(Socket socket, Stream socketStream = null, BasePacketFactory<TPacketType, TPacketIDType, TSerializer, TDeserializer> factory = null)
            where TPacketTransmission : SocketPacketTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer>, new()
            => new TPacketTransmission() { Socket = socket, Stream = socketStream ?? new SocketClientStream(socket), Factory = factory };

        protected Socket Socket { get; private set; }

        public virtual string Host => (Socket.RemoteEndPoint as IPEndPoint)?.Address.ToString() ?? string.Empty;
        public virtual ushort Port => (ushort) ((Socket.RemoteEndPoint as IPEndPoint)?.Port ?? 0);
        public virtual bool IsConnected => Socket?.Connected == true;
        public override long AvailableData => Socket?.Available ?? -1;

        protected SocketPacketTransmission() { }

        protected SocketPacketTransmission(Socket socket, Stream socketStream = null, BasePacketFactory<TPacketType, TPacketIDType, TSerializer, TDeserializer> factory = null) : base(socketStream ?? new SocketClientStream(socket), factory)
        {
            Socket = socket;
        }

        public virtual void Connect(string ip, ushort port) { Socket.Connect(ip, port); }
        public virtual void Disconnect() { Socket.Disconnect(false); }

        protected virtual void Send(byte[] buffer)
        {
            Stream.Write(buffer, 0, buffer.Length);
        }
        protected virtual byte[] Receive(long length)
        {
            var buffer = new byte[length];
            Stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        //protected virtual long ReadPacketLength() => VarInt.Decode(Stream);

        public override void SendPacket(TPacketType packet)
        {
            using var serializer = new TSerializer();
            serializer.Write(packet.ID);
            packet.Serialize(serializer);

            Send(serializer.GetData());
            /*
            Span<byte> packetData = serializer.GetData();
            Span<byte> length = new VarInt(packetData.Length).Encode();

            Send(length.ToArray());
            Send(packetData.ToArray());
            */
        }

        public override TPacketType ReadPacket()
        {
            if (Socket.Available > 0)
            {
                using var deserializer = StreamDeserializer.Create<TDeserializer>(Stream);
                var id = deserializer.Read<TPacketIDType>();
                var packet = Factory.Create(id);
                if (packet != null)
                {
                    packet.Deserialize(deserializer);
                    return packet;
                }
            }

            return null;
        }

        public override void Dispose() { }
    }
}