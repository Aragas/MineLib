using App.Metrics.Health;
using App.Metrics.Health.Checks.Sql;

using Aragas.QServer.Core;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.IO;
using Aragas.QServer.Core.NetworkBus.Messages;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Server.Core;
using MineLib.Server.Core.NetworkBus.Messages;
using MineLib.Server.Core.Packets.WorldBus;

using Npgsql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.WorldBus
{
    internal sealed class Program : MineLibProgram
    {
        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        public IWorldHandler WorldHandler { get; } = new StandardWorldHandler();

        private ManualResetEvent Waiter { get; } = new ManualResetEvent(false);
        private CompositeDisposable Events { get; } = new CompositeDisposable();

        public Program() : base(healthConfigure: ConfigureHealth)
        {
            Events.Add(BaseSingleton.Instance.SubscribeAndReply<ServicesPingMessage>(_ =>
                new ServicesPongMessage() { ServiceId = ProgramGuid, ServiceType = "WorldBus" }));

            IEnumerable<ChunksInCircleResponseMessage> GetChunksInCircleRequestEnumerable(ChunksInCircleRequestMessage message)
            {
                foreach (var chunkElement in GetChunksInCircleRequest(message.X, message.Z, message.Radius).Detailed())
                {
                    var serializer = new CompressedProtobufSerializer();
                    serializer.Write(chunkElement.Value);
                    yield return new ChunksInCircleResponseMessage(chunkElement.IsLast) { Data = serializer.GetData().ToArray() };
                }
            }
            Events.Add(BaseSingleton.Instance.SubscribeAndReplyEnumerable<ChunksInCircleRequestMessage, ChunksInCircleResponseMessage>(GetChunksInCircleRequestEnumerable));

            IEnumerable<ChunksInSquareResponseMessage> GetChunksInSquareRequestEnumerable(ChunksInSquareRequestMessage message)
            {
                foreach (var chunkElement in GetChunksInSquareRequest(message.X, message.Z, message.Radius).Detailed())
                {
                    var serializer = new CompressedProtobufSerializer();
                    serializer.Write(chunkElement.Value);
                    yield return new ChunksInSquareResponseMessage(chunkElement.IsLast) { Data = serializer.GetData().ToArray() };
                }
            }
            Events.Add(BaseSingleton.Instance.SubscribeAndReplyEnumerable<ChunksInSquareRequestMessage, ChunksInSquareResponseMessage>(GetChunksInSquareRequestEnumerable));
        }
        public static IHealthBuilder ConfigureHealth(IHealthBuilder builder) => builder
            .HealthChecks.AddProcessPhysicalMemoryCheck("Process Working Set Size", 100 * 1024 * 1024)
            .HealthChecks.AddProcessPrivateMemorySizeCheck("Process Private Memory Size", 100 * 1024 * 1024)
            .HealthChecks.AddSqlCheck("PosgreSQL Connection", () => new NpgsqlConnection(MineLibSingleton.PostgreSQLConnectionString), TimeSpan.FromMilliseconds(500));

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Waiter.WaitOne();
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            InternalBus.WorldBus.MessageReceived -= WorldBus_MessageReceived;

            Waiter.Set();
        }

        private IEnumerable<Chunk> GetChunksInCircleRequest(int x0, int z0, int radius)
        {
            for (int x = x0 - radius; x <= x0 + radius; x++)
                for (int z = z0 - radius; z <= z0 + radius; z++)
                    if (((x - x0) * (x - x0)) + ((z - z0) * (z - z0)) <= radius * radius)
                        yield return WorldHandler.GetChunk(new Location2D(x, z));
        }
        private IEnumerable<Chunk> GetChunksInSquareRequest(int x0, int z0, int radius)
        {
            /*
            using var fs = new FileStream("D:\\clotvostok v.2\\region\\r.0.0.mcr", FileMode.Open, FileAccess.Read);
            using var br = new BinaryReader(fs);

            var locationTable = new List<(int Offset, int SectorCount)>();
            for (int i = 0; i < 1024; i++)
                locationTable.Add((br.ReadByte() << 16 | br.ReadByte() << 8 | br.ReadByte(), br.ReadByte()));

            var timestampTable = new List<int>();
            for (int i = 0; i < 1024; i++)
                timestampTable.Add(br.ReadInt32());


            foreach (var (Offset, SectorCount) in locationTable.Take(20))
            {
                if (Offset == 0 && SectorCount == 0)
                    continue;

                fs.Seek(Offset * 4096, SeekOrigin.Begin);
                var length = br.ReadInt32();
                var compressionType = br.ReadByte();
                var nbt = new NbtFile();
                nbt.LoadFromStream(fs, NbtCompression.AutoDetect, null);

                var nbtTags = nbt.RootTag.Tags.Cast<NbtCompound>().First();

                var blocks = nbtTags["Blocks"].ByteArrayValue;
                var meta = nbtTags["Metadata"].ByteArrayValue;
                var chunk = new Chunk(new Location2D(nbtTags["xPos"].IntValue, nbtTags["zPos"].IntValue));

                //var biomes = nbtTags["BiomeData"].ByteArrayValue;
                //for (var i = 0; i < 256; i++)
                //    chunk.Biomes[i] = biomes[i];

                var index = 0;
                for (var x = 0; x < 16; x++)
                for (var z = 0; z < 16; z++)
                for (var y = 0; y < 128; y++)
                    chunk.SetBlock(new Location3D(x, y, z), new ReadonlyBlock32(blocks[index], meta[index++]));
                yield return chunk;
            }
            */

            for (int x = x0 - radius; x <= x0 + radius; x++)
                for (int z = z0 - radius; z <= z0 + radius; z++)
                    yield return WorldHandler.GetChunk(new Location2D(x, z));
        }

        private void WorldBus_MessageReceived(object? sender, MBusMessageReceivedEventArgs args)
        {
            IEnumerable<ChunkEnumerableResponse> GetChunksInCircleRequestEnumerable(ChunksInCircleRequest request)
            {
                if (request.SendBulk)
                    yield break;

                foreach(var chunk in GetChunksInCircleRequest(request.X, request.Z, request.Radius))
                    yield return new ChunkEnumerableResponse() { Chunk = chunk };
            }
            IEnumerable<ChunkEnumerableResponse> GetChunksInSquareRequestEnumerable(ChunksInSquareRequest request)
            {
                if (request.SendBulk)
                    yield break;

                foreach (var chunk in GetChunksInSquareRequest(request.X, request.Z, request.Radius))
                    yield return new ChunkEnumerableResponse() { Chunk = chunk };
            }

            InternalBus.HandleRequest<SectionRequest, SectionResponse>(InternalBus.WorldBus, args, request => new SectionResponse()
            {
                Section = WorldHandler.GetSection(request.Position)
            });
            InternalBus.HandleRequest<ChunkRequest, ChunkResponse>(InternalBus.WorldBus, args, request => new ChunkResponse()
            {
                Chunk = WorldHandler.GetChunk(request.Coordinates)
            });
            InternalBus.HandleRequest<ChunksInCircleRequest, ChunkBulkResponse>(InternalBus.WorldBus, args, request =>
            {
                if (request.SendBulk)
                    return new ChunkBulkResponse() { ChunkBulk = GetChunksInCircleRequest(request.X, request.Z, request.Radius).ToArray() };
                return null;
            });
            InternalBus.HandleRequest<ChunksInSquareRequest, ChunkBulkResponse>(InternalBus.WorldBus, args, request =>
            {
                if (request.SendBulk)
                    return new ChunkBulkResponse() { ChunkBulk = GetChunksInSquareRequest(request.X, request.Z, request.Radius).ToArray() };
                return null;
            });
            InternalBus.HandleRequest<ChunksInCircleRequest, ChunkEnumerableResponse>(InternalBus.WorldBus, args, GetChunksInCircleRequestEnumerable);
            InternalBus.HandleRequest<ChunksInSquareRequest, ChunkEnumerableResponse>(InternalBus.WorldBus, args, GetChunksInSquareRequestEnumerable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Waiter.Dispose();
                Events.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}