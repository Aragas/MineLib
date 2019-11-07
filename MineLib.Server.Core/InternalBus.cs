using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;

using Aragas.Network.Data;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Server.Core.Extensions;
using MineLib.Server.Core.Packets;
using MineLib.Server.Core.Packets.EntityBus;
using MineLib.Server.Core.Packets.PlayerHandler;
using MineLib.Server.Core.Packets.WorldBus;
using MineLib.Server.Core.Protocol;

namespace MineLib.Server.Core
{
    // Warning, you can't use the same instance for sending and getting the same message
    public static class InternalBus
    {
        public static string Host { get; } = "mbus://localhost";

        public const int Timeout = 10000;

        private static IMBus? _worldBus;
        public static IMBus WorldBus => _worldBus ?? (_worldBus = new NetworkMBus($"{Host}/minelib/server/worldbus", TimeSpan.FromMilliseconds(Timeout)));
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
        public static IMBus EntityBus => _entityBus ?? (_entityBus = new NetworkMBus($"{Host}/minelib/server/entitybus"));
        public static int? GetEntityID(IMBus? entityBus = null)
        {
            return HandleResponse<int?, EntityIDRequest, EntityIDResponse>(entityBus ?? EntityBus,
                new EntityIDRequest(),
                response => response.EntityID);
        }

        private static IMBus? _playerBus;
        public static IMBus PlayerBus => _playerBus ?? (_playerBus = new NetworkMBus($"{Host}/minelib/server/playerbus"));
        public static Socket GetFirstAvailablePlayerHandlerConnection(VarInt protocolVersion, IMBus? playerBus = null)
        {
            return HandleResponse<Socket, AvailableSocketRequestPacket, AvailableSocketResponsePacket>(playerBus ?? PlayerBus,
                new AvailableSocketRequestPacket() { ProtocolVersion = protocolVersion },
                response =>
                {
                    var conn = new TcpClient();
                    conn.Connect(response.Endpoint);
                    return conn.Client;
                });
        }
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
        public static IMBus ForgeBus => _forgeBus ?? (_forgeBus = new NetworkMBus($"{Host}/minelib/server/forgebus"));


        public static int SquareSize(int radius) => (int) Math.Pow(Math.Pow(radius, 2) + 1, 2);
        public static int CircleSize(int radius) => (int) Math.Floor(Math.Pow(radius, 2) * Math.PI);

        public static TReturn HandleResponse<TReturn, TPacketRequest, TPacketResponse>(IMBus bus, TPacketRequest request, Func<TPacketResponse, TReturn> responseHandler, int timeout = Timeout)
            where TPacketRequest : InternalPacket
            where TPacketResponse : InternalPacket
        {
            var @return = default(TReturn);
            var guid = Guid.NewGuid();

            using var eventLock = new ManualResetEventSlim(false);
            bus.MessageReceived += (sender, args) =>
            {
                using var internalFactory = new InternalFactory();
                switch (internalFactory.GetPacket(args.Message))
                {
                    case PingPacket pingPacket:
                        break;

                    case TPacketResponse response:
                        if (!guid.Equals(response.GUID))
                            return;

                        @return = responseHandler(response);
                        eventLock.Set();
                        break;
                }
            };

            request.GUID = guid;
            bus.SendPacket(request);

            eventLock.Wait(timeout);

            return @return;
        }
        public static IEnumerable<TReturn> HandleResponse<TReturn, TPacketRequest, TPacketEnumerableResponse, TPacketBulkResponse>(
            IMBus bus,
            TPacketRequest request,
            Func<TPacketEnumerableResponse, TReturn> responseEnumerableHandler,
            Func<TPacketBulkResponse, TReturn[]> responseBulkHandler,
            bool inBulk = false, int timeout = Timeout)
            where TPacketRequest : InternalPacket
            where TPacketEnumerableResponse : InternalPacket
            where TPacketBulkResponse : InternalPacket
        {
            var bulk = Array.Empty<TReturn>();
            var currentReturn = default(TReturn);
            using var lock1 = new ManualResetEventSlim(false);
            using var lock2 = new ManualResetEventSlim(true);
            var lastPacket = DateTime.UtcNow;

            var guid = Guid.NewGuid();

            using var eventLock = new ManualResetEventSlim(false);
            bus.MessageReceived += (sender, args) =>
            {
                using var internalFactory = new InternalFactory();
                switch (internalFactory.GetPacket(args.Message))
                {
                    case PingPacket pingPacket:
                        break;

                    case TPacketEnumerableResponse response:
                        if (!guid.Equals(response.GUID))
                            return;

                        lock2.Wait(); // Ждем завершения отправки данных
                        currentReturn = responseEnumerableHandler(response); // Ставим новые данные
                        lastPacket = DateTime.UtcNow;
                        lock2.Reset();
                        lock1.Set(); // Завершили отправку данных
                        break;

                    case TPacketBulkResponse response:
                        if (!guid.Equals(response.GUID))
                            return;

                        bulk = responseBulkHandler(response);
                        eventLock.Set();
                        break;
                }
            };

            request.GUID = guid;
            bus.SendPacket(request);

            if (inBulk)
            {
                eventLock.Wait(timeout * 2);

                foreach (var item in bulk)
                    yield return item;
            }
            else
            {
                while (DateTime.UtcNow - lastPacket < TimeSpan.FromMilliseconds(timeout))
                {
                    lock1.Wait(timeout); // Ждем пока появятся данные
                    yield return currentReturn; // Поставили данные
                    lock1.Reset();
                    lock2.Set(); // Завершили отправку данных
                }
            }
        }
        public static void HandleRequest<TPacketRequest, TPacketResponse>(IMBus bus, MBusMessageReceivedEventArgs args, Func<TPacketRequest, TPacketResponse?> requestHandler, int timeout = Timeout)
            where TPacketRequest : InternalPacket
            where TPacketResponse : InternalPacket
        {
            using var internalFactory = new InternalFactory();
            switch (internalFactory.GetPacket(args.Message))
            {
                case TPacketRequest request:
                    var response = requestHandler(request);
                    if (response == null) return;
                    response.GUID = request.GUID;
                    bus.SendPacket(response);
                    break;
            }
        }
        public static void HandleRequest<TPacketRequest, TPacketResponse>(IMBus bus, MBusMessageReceivedEventArgs args, Func<TPacketRequest, IEnumerable<TPacketResponse>?> requestHandler, int timeout = Timeout)
            where TPacketRequest : InternalPacket
            where TPacketResponse : InternalPacket
        {
            using var internalFactory = new InternalFactory();
            switch (internalFactory.GetPacket(args.Message))
            {
                case TPacketRequest request:
                    var responseEnumerable = requestHandler(request);
                    if (responseEnumerable == null) return;
                    foreach (var response in responseEnumerable)
                    {
                        response.GUID = request.GUID;
                        bus.SendPacket(response);
                    }
                    break;
            }
        }


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