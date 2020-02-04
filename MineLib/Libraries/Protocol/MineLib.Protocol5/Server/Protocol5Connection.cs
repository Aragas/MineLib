using Aragas.QServer.Core;
using Aragas.QServer.Core.IO;
using Aragas.QServer.Core.NetworkBus;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Protocol.Netty;
using MineLib.Protocol.Packets;
using MineLib.Protocol5.Extensions;
using MineLib.Protocol5.Packets.Client.Login;
using MineLib.Protocol5.Packets.Client.Play;
using MineLib.Protocol5.Packets.Server.Login;
using MineLib.Protocol5.Packets.Server.Play;
using MineLib.Server.Core.NetworkBus.Messages;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
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

        private IAsyncNetworkBus NetworkBus { get; }
        private Protocol5Transmission Stream { get; }
        private ConcurrentQueue<MinecraftEnumPacket> PacketsToSend { get; } = new ConcurrentQueue<MinecraftEnumPacket>();

        public Protocol5Connection(IAsyncNetworkBus networkBus, Guid playerId, State state = Protocol.Netty.State.Handshake)
        {
            NetworkBus = networkBus;

            PlayerId = playerId;
            Stream = new Protocol5Transmission(networkBus, playerId, state);
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
                                    var response = NetworkBus.PublishAndWaitForReplyEnumerableAsync<ChunksInSquareRequestMessage, ChunksInSquareResponseMessage>(
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
                            }
                            break;
                        case PlayerPositionPacket packet:
                            {
                                var response = NetworkBus.PublishAndWaitForReply<PlayerPositionRequestMessage, PlayerPositionResponseMessage>(new PlayerPositionRequestMessage()
                                {
                                    PlayerId = PlayerId,
                                    Position = new Vector3((float)packet.X, (float)packet.FeetY, (float)packet.Z)
                                });
                                if(response?.IsCorrect == false)
                                {
                                    PacketsToSend.Enqueue(new PlayerPositionAndLookPacket()
                                    {
                                        X = response.Position.X,
                                        Y = response.Position.Y,
                                        Z = response.Position.Z,
                                    });
                                }
                            }
                            break;
                        case PlayerLookPacket packet:
                            {
                                var response = NetworkBus.PublishAndWaitForReply<PlayerLookRequestMessage, PlayerLookResponseMessage>(new PlayerLookRequestMessage()
                                {
                                    PlayerId = PlayerId,
                                    Look = new Look(packet.Pitch, packet.Yaw),
                                });
                                if (response?.IsCorrect == false)
                                {
                                    PacketsToSend.Enqueue(new PlayerPositionAndLookPacket()
                                    {
                                        Yaw = response.Look.Yaw,
                                        Pitch = response.Look.Pitch
                                    });
                                }
                            }
                            break;
                        case PlayerPositionAndLook2Packet packet:
                            {
                                var responsePositionTask = NetworkBus.PublishAndWaitForReplyAsync<PlayerPositionRequestMessage, PlayerPositionResponseMessage>(new PlayerPositionRequestMessage()
                                {
                                    PlayerId = PlayerId,
                                    Position = new Vector3((float) packet.X, (float) packet.FeetY, (float) packet.Z)
                                });
                                var responseLookTask = NetworkBus.PublishAndWaitForReplyAsync<PlayerLookRequestMessage, PlayerLookResponseMessage>(new PlayerLookRequestMessage()
                                {
                                    PlayerId = PlayerId,
                                    Look = new Look(packet.Pitch, packet.Yaw),
                                });

                                Task.WaitAll(responsePositionTask, responseLookTask);
                                var responsePosition = responsePositionTask.Result;
                                var responseLook = responseLookTask.Result;
                                if (responsePosition?.IsCorrect == false && responseLook?.IsCorrect == false)
                                {
                                    PacketsToSend.Enqueue(new PlayerPositionAndLookPacket()
                                    {
                                        X = responsePosition.Position.X,
                                        Y = responsePosition.Position.Y,
                                        Z = responsePosition.Position.Z,
                                        Yaw = responseLook.Look.Yaw,
                                        Pitch = responseLook.Look.Pitch
                                    });
                                }
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