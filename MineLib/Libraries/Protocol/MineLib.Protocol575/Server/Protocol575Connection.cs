using Aragas.Network.Data;
using Aragas.QServer.Core.IO;
using Aragas.QServer.NetworkBus;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Protocol.Netty;
using MineLib.Protocol.Netty.Protocol;
using MineLib.Protocol.Packets;
using MineLib.Protocol5.Extensions;
using MineLib.Protocol575.Packets;
using MineLib.Protocol575.Packets.Client.Login;
using MineLib.Protocol575.Packets.Client.Play;
using MineLib.Protocol575.Packets.Server.Login;
using MineLib.Server.Core.NetworkBus.Messages;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        private IAsyncNetworkBus NetworkBus { get; }
        private ProtocolNettyTransmission<ServerStatusPacket, ServerLoginPacket, ServerPlayPacket> Stream { get; }
        private ConcurrentQueue<MinecraftPacket> PacketsToSend { get; } = new ConcurrentQueue<MinecraftPacket>();

        public Protocol575Connection(IAsyncNetworkBus networkBus, Guid playerId, State state = Protocol.Netty.State.Handshake)
        {
            NetworkBus = networkBus;

            PlayerId = playerId;
            Stream = new ProtocolNettyTransmission<ServerStatusPacket, ServerLoginPacket, ServerPlayPacket>(networkBus, playerId, state);
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

                                Task.Factory.StartNew(async () =>
                                {
                                    PacketsToSend.Enqueue(new PlayerPositionAndLookPacket()
                                    {
                                        X = 7,
                                        Y = 62 + 1.62D,
                                        Z = 7,
                                        Yaw = 0,
                                        Pitch = 0,
                                    });

                                    PacketsToSend.Enqueue(new UpdateViewPositionPacket()
                                    {
                                        X = 0,
                                        Z = 0
                                    });

                                    var response = NetworkBus.PublishAndWaitForReplyEnumerableAsync<ChunksInSquareRequestMessage, ChunksInSquareResponseMessage>(
                                        new ChunksInSquareRequestMessage() { X = 0, Z = 0, Radius = 1 });
                                    var chunks = response.Select(r =>
                                    {
                                        var deserializer = new CompressedProtobufDeserializer(r.Data);
                                        return deserializer.Read<Chunk>();
                                    });

                                    await foreach (var chunk in chunks)
                                    {
                                        var chunkPacket = chunk.CreatePacket();
                                        var lightPacket = chunk.CreateLightPacket();
                                        PacketsToSend.Enqueue(chunkPacket);
                                        PacketsToSend.Enqueue(lightPacket);
                                    }
                                    //var chunk = await chunks.ToListAsync();
                                    //var data = chunk[0].CreatePacket();
                                    //PacketsToSend.Enqueue(data);
                                    PacketsToSend.Enqueue(new WorldBorderPacket()
                                    {
                                        X = 0,
                                        Z = 0,
                                        NewDiameter = 1000,
                                        OldDiameter = 1000,
                                        PortalTeleportBoundary = 29999984,
                                        Speed = new VarLong(0),
                                        WarningTime = 30,
                                        WarningBlocks = 30
                                    });

                                    await Task.Factory.StartNew(() =>
                                    {
                                        PacketsToSend.Enqueue(new SpawnPositionPacket() { Location = new Location3D(7, 62, 7) });
                                        PacketsToSend.Enqueue(new PlayerPositionAndLookPacket()
                                        {
                                            X = 7,
                                            Y = 62 + 1.62D,
                                            Z = 7,
                                            Yaw = 0,
                                            Pitch = 0,
                                        });
                                    });
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
                        PacketsToSend.Enqueue(new KeepAlivePacket() { KeepAliveID = 0 });
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