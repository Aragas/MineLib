using Aragas.QServer.Core;
using Aragas.QServer.Core.IO;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Protocol.Netty;
using MineLib.Protocol.Packets;
using MineLib.Protocol5.Extensions;
using MineLib.Protocol5.Packets.Client.Login;
using MineLib.Protocol5.Packets.Client.Play;
using MineLib.Protocol5.Packets.Server.Login;
using MineLib.Protocol5.Packets.Server.Play;
using MineLib.Server.Core;
using MineLib.Server.Core.NetworkBus.Messages;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Protocol5.Server
{
    public class Protocol5Connection : BaseProtocol5Connection
    {
        static Protocol5Connection()
        {
            PacketExtensions.Init();
        }

        private Guid PlayerId;
        private string Username;

        public override int State => (int) Stream.State;

        public override string Host => string.Empty;
        public override ushort Port => 0;
        public override bool Connected => true;

        private Protocol5Transmission Stream { get; }
        private ConcurrentQueue<MinecraftPacket> PacketsToSend { get; } = new ConcurrentQueue<MinecraftPacket>();

        public Protocol5Connection(Guid playerId, State state = Protocol.Netty.State.Handshake)
        {
            PlayerId = playerId;
            Stream = new Protocol5Transmission()
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
                                /*
                                var player = InternalBus.GetPlayerData(packet.Name, PlayerBus);
                                if (player == null)
                                {
                                    Disconnect();
                                    break;
                                }
                                */

                                PacketsToSend.Enqueue(new LoginSuccessPacket()
                                {
                                    Username = (Username = player.Username),
                                    UUID = player.Uuid.ToString()
                                });
                                PacketsToSend.Enqueue(new JoinGamePacket()
                                {
                                    Difficulty = 0,
                                    Dimension = 0,
                                    //EntityID = ProtocolBus.GetEntityID() ?? 1,
                                    //EntityID = InternalBus.GetEntityID() ?? 1,
                                    EntityID = 1,
                                    GameMode = 1,
                                    LevelType = "flat",
                                    MaxPlayers = 10
                                });

                                //PacketsToSend.Enqueue(Forge.)

                                Task.Factory.StartNew(async () =>
                                {
                                    var response = BaseSingleton.Instance
                                    .PublishAndWaitForReplyEnumerableAsync<ChunksInSquareRequestMessage, ChunksInSquareResponseMessage>(
                                        new ChunksInSquareRequestMessage() { X = 0, Z = 0, Radius = 5 });
                                    var chunks = response.Select(r =>
                                    {
                                        var deserializer = new CompressedProtobufDeserializer(r.Data);
                                        return deserializer.Read<Chunk>();
                                    });
                                    //await foreach (var chunk in chunks)
                                    //    PacketsToSend.Enqueue(chunk.CreatePacket());
                                    var array = await chunks.ToArrayAsync();
                                    var bulk = array.MapChunkBulk();
                                    PacketsToSend.Enqueue(bulk);


                                    await Task.Factory.StartNew(() =>
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
                                Task.Factory.StartNew(() =>
                                {
                                    InternalBus.UpdatePlayerData(new Core.Player()
                                    {
                                        Position = new System.Numerics.Vector3((float)packet.X, (float)packet.FeetY, (float)packet.Z),
                                        Username = Username
                                    }, PlayerBus);
                                });
                            }
                            break;
                        case PlayerLookPacket packet:
                            {
                                Task.Factory.StartNew(() =>
                                {
                                    InternalBus.UpdatePlayerData(new Core.Player()
                                    {
                                        Look = new Core.Look(packet.Pitch, packet.Yaw),
                                        Username = Username
                                    }, PlayerBus);
                                });
                            }
                            break;
                        case PlayerPositionAndLook2Packet packet:
                            {
                                Task.Factory.StartNew(() =>
                                {
                                    InternalBus.UpdatePlayerData(new Core.Player()
                                    {
                                        Position = new System.Numerics.Vector3((float)packet.X, (float)packet.FeetY, (float)packet.Z),
                                        Look = new Core.Look(packet.Pitch, packet.Yaw),
                                        Username = Username
                                    }, PlayerBus);
                                });
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