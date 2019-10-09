using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using MineLib.Protocol.Packets;
using MineLib.Protocol5.Extensions;
using MineLib.Protocol5.Packets.Client.Login;
using MineLib.Protocol5.Packets.Client.Play;
using MineLib.Protocol5.Packets.Server.Login;
using MineLib.Protocol5.Packets.Server.Play;
using MineLib.Protocol5.Protocol;
using MineLib.Server.Core;

namespace MineLib.Protocol5.Server
{
    public partial class Protocol5Connection : BaseProtocol5Connection
    {
        static Protocol5Connection()
        {
            PacketExtensions.Init();
        }


        public override string Host => Stream?.Host ?? string.Empty;
        public override ushort Port => Stream?.Port ?? 0;
        public override bool Connected => Stream?.IsConnected ?? false;

        private Protocol5Transmission Stream { get; }
        private ConcurrentQueue<MinecraftPacket> PacketsToSend { get; } = new ConcurrentQueue<MinecraftPacket>();

        public Protocol5Connection(Socket proxyConnection, State state = State.Handshake)
        {
            Stream = new Protocol5Transmission()
            {
                State = state,
                Socket = proxyConnection
            };
            new Thread(PacketReceiver).Start();
        }

        private Stopwatch Stopwatch { get; set; }
        private void PacketReceiver()
        {
            while (Stream.IsConnected)
            {
                while (Stream.TryReadPacket(out var packetToReceive))
                {
                    switch (packetToReceive)
                    {
                        case LoginStartPacket packet:
                            {
                                PacketsToSend.Enqueue(new LoginSuccessPacket()
                                {
                                    Username = packet.Name,
                                    UUID = Guid.NewGuid().ToString()
                                });
                                PacketsToSend.Enqueue(new JoinGamePacket()
                                {
                                    Difficulty = 0,
                                    Dimension = 0,
                                    //EntityID = ProtocolBus.GetEntityID() ?? 1,
                                    EntityID = InternalBus.GetEntityID() ?? 1,
                                    GameMode = 1,
                                    LevelType = "flat",
                                    MaxPlayers = 10
                                });

                                //PacketsToSend.Enqueue(Forge.)

                                Task.Factory.StartNew(() =>
                                {
                                    PacketsToSend.Enqueue(InternalBus.GetChunksInSquare(0, 0, 3, true).ToArray().MapChunkBulk());

                                    Task.Factory.StartNew(() =>
                                    {
                                        PacketsToSend.Enqueue(new SpawnPositionPacket() { X = 7, Y = 62, Z = 7 });
                                        PacketsToSend.Enqueue(new PlayerAbilitiesPacket()
                                        {
                                            Flags = 0,//1 & 2 & 4,
                                        FlyingSpeed = 0,
                                            WalkingSpeed = 1
                                        });
                                        PacketsToSend.Enqueue(new PlayerPositionAndLookPacket()
                                        {
                                            X = 7,
                                            Y = 62 + 1.62D,
                                            Z = 7,
                                            Yaw = 0,
                                            Pitch = 0,
                                            OnGround = true
                                        });
                                    });

                                //PacketsToSend.Enqueue(InternalBus.GetChunk(new Coordinates2D(0, 0))?.CreatePacket());
                                //PacketsToSend.Enqueue(InternalBus.GetChunksInSquare(0, 0, 12, true).ToArray().MapChunkBulk());
                            });

                                /*
                                PacketsToSend.Enqueue(InternalBus.GetChunk(new Coordinates2D(0, 0))?.CreatePacket());
                                Task.Factory.StartNew(() =>
                                {
                                    PacketsToSend.Enqueue(InternalBus.GetChunksInRadius(0, 0, 0, 0, 0).MapChunkBulk());
                                });
                                */
                            }
                            break;
                        case PlayerPositionPacket packet:
                            {
                            }
                            break;
                        case PlayerLookPacket packet:
                            {
                            }
                            break;
                        case PlayerPositionAndLook2Packet packet:
                            {
                                /*
                                var valid = InternalBus.ValidatePlayerPositionAndLook(
                                    new MineLib.Protocol.IPC.Packets.PlayerDataPacket()
                                    {
                                        Yaw = pplPacket.Yaw,
                                        Pitch = pplPacket.Pitch,
                                        X = pplPacket.X,
                                        FeetY = pplPacket.FeetY,
                                        Z = pplPacket.Z,
                                        OnGround = pplPacket.OnGround
                                    });
                                */
                                /*
                                if (valid)
                                {
                                    yaw = pplPacket.Yaw;
                                    pitch = pplPacket.Pitch;
                                    x = pplPacket.X;
                                    y = pplPacket.FeetY;
                                    z = pplPacket.Z;
                                }

                                if (!valid)
                                {
                                    PacketsToSend.Enqueue(new PlayerPositionAndLookPacket()
                                    {
                                        Yaw = (float) yaw,
                                        Pitch = (float) pitch,
                                        X = x,
                                        Y = y,
                                        Z = z,
                                        OnGround = true
                                    });
                                }
                                */
                            }
                            break;

                        case PlayerBlockPlacementPacket packet:
                            {

                            }
                            break;

                        case PluginMessage2Packet packet:
                            {
                                //Forge.HandlePluginMessage(packet.Channel, packet.Data);
                            }
                            break;
                    }


                    PacketsReceived.Add(packetToReceive);
                    //DoCustomReceiving(packetToReceive);
                }

                while (PacketsToSend.TryDequeue(out var packetToSend))
                {
                    if (packetToSend is null)
                        continue;

                    Stream.SendPacket(packetToSend);
                    PacketsSended.Add(packetToSend);
                }

                if (Stream.State == State.Play)
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