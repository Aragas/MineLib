using Aragas.QServer.Core;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Server.Core;
using MineLib.Server.Core.Packets.WorldBus;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MineLib.Server.WorldBus
{
    internal sealed class Program : MineLibProgram
    {
        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        public IWorldHandler WorldHandler { get; } = new StandardWorldHandler();

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Console.WriteLine($"MineLib.Server.WorldBus");

            InternalBus.WorldBus.MessageReceived += (this, WorldBus_MessageReceived);

            Console.ReadLine();
            await StopAsync().ConfigureAwait(false);
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            InternalBus.WorldBus.MessageReceived -= WorldBus_MessageReceived;
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
                InternalBus.WorldBus.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}