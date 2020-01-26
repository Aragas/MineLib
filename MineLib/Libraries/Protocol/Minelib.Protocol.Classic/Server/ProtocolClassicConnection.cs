using Aragas.QServer.Core.NetworkBus;

using MineLib.Protocol.Packets;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace MineLib.Protocol.Classic.Server
{
    public partial class ProtocolClassicConnection : BaseProtocolClassicConnection
    {
        static ProtocolClassicConnection()
        {
            //PacketExtensions.Init();
        }

        private Guid PlayerId;
        private string Username;

        public override int State => 0;// (int) Stream.State;

        public override string Host => string.Empty;
        public override ushort Port => 0;
        public override bool Connected => true;

        private IAsyncNetworkBus NetworkBus { get; }
        //private ProtocolNettyTransmission<ServerStatusPacket, ServerLoginPacket, ServerPlayPacket> Stream { get; }
        private ConcurrentQueue<MinecraftPacket> PacketsToSend { get; } = new ConcurrentQueue<MinecraftPacket>();

        public ProtocolClassicConnection(IAsyncNetworkBus networkBus, Guid playerId)
        {
            NetworkBus = networkBus;

            PlayerId = playerId;
            /*
            Stream = new ProtocolNettyTransmission<ServerStatusPacket, ServerLoginPacket, ServerPlayPacket>()
            {
                State = state,
                PlayerId = playerId
            };
            */
            new Thread(PacketReceiver).Start();
        }

        private Stopwatch? Stopwatch { get; set; }
        private void PacketReceiver()
        {
            while (true)
            {
                /*
                while (Stream.TryReadPacket(out var packetToReceive) && packetToReceive != null)
                {
                    switch (packetToReceive)
                    {
                        case null:
                            break;
                    }


#if DEBUG
                    PacketsReceived.Add(packetToReceive);
#endif
                }

                while (PacketsToSend.TryDequeue(out var packetToSend))
                {
                    if (packetToSend is null)
                        continue;

                    Stream.SendPacket(packetToSend);
#if DEBUG
                    PacketsSended.Add(packetToSend);
#endif
                }
                */

                Thread.Sleep(15);
            }
        }

        public override void Disconnect()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}