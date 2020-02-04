using Aragas.Network.IO;
using Aragas.Network.Packets;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Messages;

using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;

namespace Aragas.QServer.Core.Protocol
{
    public abstract class SocketPacketINetworkBusTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer> :
        SocketPacketTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer>
        where TPacketType : Packet<TPacketIDType>
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

                Events.Add(_networkBus.Subscribe<PlayerDataToBusMessage>(message => DataReceivedQueue.Enqueue(message.Data), PlayerId));
            }
        }

        private ConcurrentQueue<byte[]> DataReceivedQueue { get; } = new ConcurrentQueue<byte[]>();

        private CompositeDisposable Events { get; } = new CompositeDisposable();

        private readonly IAsyncNetworkBus _networkBus;

        protected SocketPacketINetworkBusTransmission(IAsyncNetworkBus networkBus, Guid playerId, BasePacketFactory<TPacketType, TPacketIDType>? defaultFactory = null)
        {
            _networkBus = networkBus;
            PlayerId = playerId;
            DefaultFactory = defaultFactory;
        }

        protected override void Send(in ReadOnlySpan<byte> span)
        {
            _networkBus.Publish(new PlayerDataToProxyMessage() { Data = span.ToArray() }, PlayerId);
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
            if (DefaultFactory == null)
                throw new NullReferenceException($"Property {nameof(DefaultFactory)} is null. Provide an implementation or override {nameof(ReadPacket)} with your implementation.");

            var data = Receive(0);
            if (data.IsEmpty)
                return null;

            using var deserializer = StreamDeserializer.Create<TDeserializer>(data);
            var id = deserializer.Read<TPacketIDType>();
            var packet = DefaultFactory.Create(id);
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
                Events.Dispose();
                DataReceivedQueue.Clear();
            }

            base.Dispose(disposing);
        }
    }
}