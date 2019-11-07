using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Core.Extensions;

namespace MineLib.Server.Core
{
    public abstract class DefaultConnectionHandler<TPacketTransmission, TPacket, TIDType, TSerializer, TDeserializer> : BaseThreadSafeConnectionHandler
        where TPacketTransmission : SocketPacketTransmission<TPacket, TIDType, TSerializer, TDeserializer>, new()
        where TPacket : Packet<TIDType, TSerializer, TDeserializer>
        where TSerializer : StreamSerializer, new()
        where TDeserializer : StreamDeserializer, new()
    {
#if DEBUG
        // -- Debug -- //
        protected const int QueueSize = 100;
        protected Queue<TPacket> Received { get; } = new Queue<TPacket>(QueueSize);
        protected Queue<TPacket> Sended { get; } = new Queue<TPacket>(QueueSize);
        // -- Debug -- //
#endif

        public TPacketTransmission Stream { get; set; }
        private ConcurrentQueue<TPacket> PacketsToSend { get; } = new ConcurrentQueue<TPacket>();

        protected DefaultConnectionHandler() { }
        protected DefaultConnectionHandler(Socket socket, BasePacketFactory<TPacket, TIDType, TSerializer, TDeserializer>? factory = null)
            => Stream = new TPacketTransmission() { Socket = socket, Factory = factory };

        public sealed override void Update()
        {
            UpdateLock.Reset(); // Signal that the UpdateThread is alive.
            try
            {
                while (!UpdateToken.IsCancellationRequested && Stream.IsConnected)
                {
                    ConnectionLock.Reset(); // Signal that we are handling pending client data.
                    try
                    {
                        while (Stream.TryReadPacket(out var packetToReceive))
                        {
                            HandlePacket(packetToReceive);

#if DEBUG
                            Received.Enqueue(packetToReceive);
                            if (Received.Count >= QueueSize)
                                Received.Dequeue();
#endif
                        }
                        while (PacketsToSend.TryDequeue(out var packetToSend))
                        {
                            Stream.SendPacket(packetToSend);

#if DEBUG
                            Sended.Enqueue(packetToSend);
                            if (Sended.Count >= QueueSize)
                                Sended.Dequeue();
#endif
                        }

                        AdditionalWork();
                    }
                    finally
                    {
                        ConnectionLock.Set(); // Signal that we are not handling anymore pending client data.
                    }

                    Thread.Sleep(100); // 10 calls per second should not be too often?
                }
            }
            finally
            {
                UpdateLock.Set(); // Signal that the UpdateThread is finished

                if (!UpdateToken.IsCancellationRequested && !Stream.IsConnected) // Leave() if the update cycle stopped unexpectedly
                    Leave();
            }
        }

        protected abstract void HandlePacket(TPacket packet);

        protected virtual void AdditionalWork() { }

        protected void SendPacket(TPacket packet) => PacketsToSend.Enqueue(packet);

        public virtual void Disconnect() => Leave();
    }
}