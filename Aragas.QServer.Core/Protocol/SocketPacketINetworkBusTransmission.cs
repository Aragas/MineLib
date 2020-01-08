using Aragas.Network.IO;
using Aragas.Network.Packets;
using Aragas.QServer.Core.NetworkBus.Messages;

using System;
using System.Collections.Concurrent;

namespace Aragas.QServer.Core.Protocol
{
    public abstract class SocketPacketINetworkBusTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer> : SocketPacketTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer>
        where TPacketType : Packet<TPacketIDType, TSerializer, TDeserializer>
        where TSerializer : StreamSerializer, new()
        where TDeserializer : StreamDeserializer, new()
    {
        private Guid? _playerId;
        public Guid PlayerId
        {
            get => _playerId!.Value; // Because we can't create a Generic with new() with properties
            set
            {
                if (_playerId != null)
                    throw new InvalidOperationException("You can't set Socket once it's initialized");
                _playerId = value;

                PlayerBusDataEvent = BaseSingleton.Instance.Subscribe<PlayerDataToBusMessage>(message => DataReceivedQueue.Enqueue(message.Data), PlayerId);
            }
        }

        private ConcurrentQueue<byte[]> DataReceivedQueue { get; } = new ConcurrentQueue<byte[]>();

        private IDisposable PlayerBusDataEvent { get; set; }

        protected SocketPacketINetworkBusTransmission() : base() { }
        protected SocketPacketINetworkBusTransmission(Guid playerId, BasePacketFactory<TPacketType, TPacketIDType, TSerializer, TDeserializer>? factory = null) : base()
        {
            PlayerId = playerId;
            Factory = factory;
        }

        protected override void Send(in ReadOnlySpan<byte> span)
        {
            BaseSingleton.Instance.Publish(new PlayerDataToProxyMessage() { Data = span.ToArray() }, PlayerId);
        }
        protected override Span<byte> Receive(long length)
        {
            return DataReceivedQueue.TryDequeue(out var data) ? data : Array.Empty<byte>();
        }

        public override void SendPacket(TPacketType packet)
        {
            using var serializer = new TSerializer();
            serializer.Write(packet.ID);
            packet.Serialize(serializer);
            Send(serializer.GetData());
        }

        public override TPacketType? ReadPacket()
        {
            if (Factory == null)
                throw new NullReferenceException($"Property {nameof(Factory)} is null. Provide an implementation or override {nameof(ReadPacket)} with your implementation.");

            var data = Receive(0);
            if (data.IsEmpty)
                return null;

            using var deserializer = StreamDeserializer.Create<TDeserializer>(data);
            var id = deserializer.Read<TPacketIDType>();
            var packet = Factory.Create(id);
            if (packet != null)
            {
                packet.Deserialize(deserializer);
                return packet;
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PlayerBusDataEvent.Dispose();
                DataReceivedQueue.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
