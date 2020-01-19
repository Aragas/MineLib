using MineLib.Core;
using MineLib.Protocol.Netty;
using MineLib.Protocol.Netty.Protocol;
using MineLib.Protocol.Packets;
using MineLib.Protocol575.Packets;
using MineLib.Protocol575.Packets.Client.Login;
using MineLib.Protocol575.Packets.Client.Play;
using MineLib.Protocol575.Packets.Server.Login;
using MineLib.Protocol575.Packets.Server.Play;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace MineLib.Protocol575.Server
{
    public partial class Protocol575Connection : BaseProtocol575Connection
    {
        static Protocol575Connection()
        {
            //PacketExtensions.Init();
        }

        private Guid PlayerId;
        private string Username;

        public override int State => (int) Stream.State;

        public override string Host => string.Empty;
        public override ushort Port => 0;
        public override bool Connected => true;

        private ProtocolNettyTransmission<ServerStatusPacketTypes, ServerLoginPacketTypes, ServerPlayPacketTypes> Stream { get; }
        private ConcurrentQueue<MinecraftEnumPacket> PacketsToSend { get; } = new ConcurrentQueue<MinecraftEnumPacket>();

        public Protocol575Connection(Guid playerId, State state = Protocol.Netty.State.Handshake)
        {
            PlayerId = playerId;
            Stream = new ProtocolNettyTransmission<ServerStatusPacketTypes, ServerLoginPacketTypes, ServerPlayPacketTypes>()
            {
                State = state,
                PlayerId = playerId
            };
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
                        case LoginStartPacket packet:
                            {
                                var player = new Player()
                                {
                                    Username = "sdfs",
                                    Uuid = Guid.NewGuid()
                                };

                                PacketsToSend.Enqueue(new LoginSuccessPacket()
                                {
                                    Username = (Username = player.Username),
                                    UUID = player.Uuid.ToString()
                                });
                                PacketsToSend.Enqueue(new JoinGamePacket()
                                {
                                    Dimension = 0,
                                    //EntityID = InternalBus.GetEntityID() ?? 1,
                                    EntityID = 1,
                                    GameMode = 1,
                                    LevelType = "flat",
                                    MaxPlayers = 10,
                                    ViewDistance = 8,
                                    ReducedDebugInfo = false
                                });
                            }
                            break;
                    }


#if DEBUG
                    PacketsReceived.Add(packetToReceive);
#endif
                    //DoCustomReceiving(packetToReceive);
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

                if (Stream.State == Protocol.Netty.State.Play)
                {
                    if (Stopwatch is null)
                        Stopwatch = Stopwatch.StartNew();

                    if (Stopwatch.ElapsedMilliseconds > 2000)
                    {
                        PacketsToSend.Enqueue(new KeepAlive2Packet() { KeepAliveID = 0 });
                        Stopwatch.Restart();
                    }
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