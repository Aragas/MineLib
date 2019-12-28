using Aragas.QServer.Core;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Server.Core.Packets.EntityBus;
using MineLib.Server.Core.Packets.PlayerHandler;
using MineLib.Server.Core.Packets.WorldBus;

using System;
using System.Collections.Generic;
using System.Numerics;

namespace MineLib.Server.Core
{
    // Warning, you can't use the same instance for sending and getting the same message
    public class InternalBus : BaseInternalBus
    {
        private static IMBus? _worldBus;
        public static IMBus WorldBus => _worldBus ?? (_worldBus = new NatsMBus($"{Host}/minelib/server/worldbus", TimeSpan.FromMilliseconds(Timeout)));
        public static object? GetWorldInfo()
        {
            // Spawn position (0, 0, 0)
            // World type (default, flat)
            // Dimension (Overworld, Neither)
            // Difficulty

            return null;
        }
        public static Chunk? GetChunk(in Location2D coordinates, IMBus? worldBus = null)
        {
            return HandleResponse<Chunk?, ChunkRequest, ChunkResponse>(worldBus ?? WorldBus,
                new ChunkRequest() { Coordinates = coordinates },
                response => response.Chunk);
        }

        public static IEnumerable<Chunk> GetChunksInCircle(int x, int z, int radius, bool inBulk = false, IMBus? worldBus = null)
        {
            return HandleResponse<Chunk, ChunksInCircleRequest, ChunkEnumerableResponse, ChunkBulkResponse>(worldBus ?? WorldBus,
                new ChunksInCircleRequest() { X = x, Z = z, Radius = radius, SendBulk = inBulk },
                response => response.Chunk,
                response => response.ChunkBulk,
                inBulk);
        }
        public static IEnumerable<Chunk> GetChunksInSquare(int x, int z, int radius, bool inBulk = false, IMBus? worldBus = null)
        {
            return HandleResponse<Chunk, ChunksInSquareRequest, ChunkEnumerableResponse, ChunkBulkResponse>(worldBus ?? WorldBus,
                new ChunksInSquareRequest() { X = x, Z = z, Radius = radius, SendBulk = inBulk },
                response => response.Chunk,
                response => response.ChunkBulk,
                inBulk);
        }

        private static IMBus? _entityBus;
        public static IMBus EntityBus => _entityBus ?? (_entityBus = new NatsMBus($"{Host}/minelib/server/entitybus"));
        public static int? GetEntityID(IMBus? entityBus = null)
        {
            return HandleResponse<int?, EntityIDRequest, EntityIDResponse>(entityBus ?? EntityBus,
                new EntityIDRequest(),
                response => response.EntityID);
        }

        private static IMBus? _playerBus;
        public static IMBus PlayerBus => _playerBus ?? (_playerBus = new NatsMBus($"{Host}/minelib/server/playerbus"));
        public static IPlayer? GetPlayerData(string username, IMBus? playerBus = null)
        {
            return HandleResponse<IPlayer?, GetPlayerDataRequestPacket, GetPlayerDataResponsePacket>(playerBus ?? PlayerBus,
                new GetPlayerDataRequestPacket() { Username = username },
                response => response.Player);
        }
        public static int UpdatePlayerData(Player player, IMBus? playerBus = null)
        {
            return HandleResponse<int, UpdatePlayerDataRequestPacket, UpdatePlayerDataResponsePacket>(playerBus ?? PlayerBus,
                new UpdatePlayerDataRequestPacket() { Player = player },
                response => response.ErrorEnum);
        }
        public static bool ValidatePlayerPositionAndLook(Vector3? position, Look? look)
        {
            if(position != null)
            {

            }
            if (look != null)
            {

            }

            return true;
        }


        private static IMBus? _forgeBus;
        public static IMBus ForgeBus => _forgeBus ?? (_forgeBus = new NatsMBus($"{Host}/minelib/server/forgebus"));


        public static int SquareSize(int radius) => (int) Math.Pow(Math.Pow(radius, 2) + 1, 2);
        public static int CircleSize(int radius) => (int) Math.Floor(Math.Pow(radius, 2) * Math.PI);


        /*
        public static IEnumerable<Chunk> GetChunksInCircle(int x, int z, int radius, bool inBulk = false)
        {
            var chunks = new ConcurrentBag<Chunk>();
            var lastPacket = DateTime.UtcNow;
            var currentChunks = 0;
            var totalChunks = CircleSize(radius);

            var guid = Guid.NewGuid();

            var eventLock = new ManualResetEventSlim(false);
            WorldBus.MessageReceived += (sender, args) =>
            {
                switch (new InternalFactory().GetPacket(args.Message))
                {
                    case PingPacket pingPacket:
                        break;

                    case ChunkEnumerableResponse response:
                        if (!guid.Equals(response.GUID)) return;

                        chunks.Add(response.Chunk);
                        lastPacket = DateTime.UtcNow;
                        currentChunks++;
                        break;

                    case ChunkBulkResponse response:
                        if (!guid.Equals(response.GUID)) return;

                        chunks = new ConcurrentBag<Chunk>(response.ChunkBulk);
                        eventLock.Set();
                        break;
                }
            };

            WorldBus.PublishPacket(new ChunksInCircleRequest() { GUID = guid, X = x, Z = z, Radius = radius, SendBulk = inBulk });

            if (inBulk)
            {
                eventLock.Wait(Timeout);

                foreach (var item in chunks)
                    yield return item;
            }
            else
            {
                while (!(chunks.IsEmpty && (currentChunks == totalChunks || DateTime.UtcNow - lastPacket > TimeSpan.FromMilliseconds(Timeout))))
                {
                    if (chunks.TryTake(out var chunk))
                        yield return chunk;
                }
            }
        }
        */
        /*
        public static IEnumerable<Chunk> GetChunkInCircle1(int x, int z, int radius)
        {
            var currentChunks = 0;
            var totalChunks = CircleSize(radius);
            var queue = new ConcurrentQueue<Chunk>();
            var lastMessage = DateTime.UtcNow;

            var guid = Guid.NewGuid();

            WorldBus.MessageReceived += (sender, args) =>
            {
                switch (new InternalFactory().GetPacket(args.Message))
                {
                    case PingPacket pingPacket:
                        break;

                    case ChunkEnumerableResponse response:
                        if (!guid.Equals(response.GUID)) return;

                        queue.Enqueue(response.Chunk.Value);
                        lastMessage = DateTime.UtcNow;
                        currentChunks++;

                        break;
                }
            };

            WorldBus.PublishPacket(new ChunkRadiusRequest() { GUID = guid, X = x, Z = z, Radius = radius });

            while (true)
            {
                if (queue.TryDequeue(out var chunk))
                    yield return chunk;

                if (queue.IsEmpty && (currentChunks == totalChunks || (DateTime.UtcNow - lastMessage > TimeSpan.FromSeconds(5))))
                    yield break;

                //Thread.Sleep(1);
            }
            yield break;
        }
        */
    }
}