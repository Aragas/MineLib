using Aragas.QServer.Core.NetworkBus;

using MineLib.Protocol.Classic.Packets;
using MineLib.Protocol.Classic.Packets.Client;
using MineLib.Protocol.Classic.Protocol;
using ProtocolClassic.Packets.Server;
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

        public override int State => 0;

        public override string Host => string.Empty;
        public override ushort Port => 0;
        public override bool Connected => true;

        private IAsyncNetworkBus NetworkBus { get; }
        private ProtocolClassicTransmission Stream { get; }
        private ConcurrentQueue<ClassicPacket> PacketsToSend { get; } = new ConcurrentQueue<ClassicPacket>();

        public ProtocolClassicConnection(IAsyncNetworkBus networkBus, Guid playerId)
        {
            NetworkBus = networkBus;

            PlayerId = playerId;
            Stream = new ProtocolClassicTransmission(networkBus, playerId);
            new Thread(PacketReceiver).Start();
        }

        private Stopwatch? Stopwatch { get; set; }
        private void PacketReceiver()
        {
            while (true)
            {
                while (Stream.TryReadPacket(out var packetToReceive) && packetToReceive != null)
                {
                    switch (packetToReceive)
                    {
                        case PlayerIdentificationPacket playerIdentification:
                            Username = playerIdentification.Username;

                            PacketsToSend.Enqueue(new ServerIdentificationPacket()
                            {
                                ProtocolVersion = playerIdentification.ProtocolVersion,
                                ServerMOTD = "",
                                ServerName = ""
                            });
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

                if (Stopwatch is null)
                    Stopwatch = Stopwatch.StartNew();

                if (Stopwatch.ElapsedMilliseconds > 2000)
                {
                    PacketsToSend.Enqueue(new PingPacket());
                    Stopwatch.Restart();
                }

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