using System;
using System.Buffers;
using System.Linq;

using Ionic.Zlib;

using MineLib.Core.Anvil;
using MineLib.Core.Extensions;
using MineLib.Protocol5.Data;
using MineLib.Protocol5.Packets.Client.Play;

namespace MineLib.Protocol5.Extensions
{
    public static class ChunkExtensions
    {
        private const int BlockDataLength = Section.Width * Section.Depth * Section.Height;
        private const int NibbleDataLength = BlockDataLength / 2;

        private const int FullBlockDataLength = BlockDataLength * Chunk.SectionCount;
        private const int FullNibbleDataLength = NibbleDataLength * Chunk.SectionCount;
        private const int FullChunkSize =
            FullBlockDataLength +       // Blocks
            FullNibbleDataLength +      // Block Metadatas
            FullNibbleDataLength +      // Block Lights
            FullNibbleDataLength +      // Block SkyLights
            //FullNibbleDataLength +      // Additional Bitmap
            (Chunk.Width * Chunk.Depth);// Biomes

        private const int SectionSize =
            BlockDataLength +       // Blocks
            NibbleDataLength +      // Block Metadatas
            NibbleDataLength +      // Block Lights
            NibbleDataLength +      // Block SkyLights
            //NibbleDataLength +      // Additional Bitmap
            0;

        private static byte[] Compress(in Span<byte> data) => ZlibStream.CompressBuffer(data.ToArray());

        public static void Serialize(this in Chunk chunk, in Span<byte> data, ref int offset)
        {
            //var sections = chunk.Sections.Where(s => !s.IsEmpty).OrderBy(s => s.Position.Y).ToArray();
            var sections = chunk.Sections.Where(s => !s.IsEmpty).ToArray();

            for (int i = 0; i < sections.Length; i++)
            {
                var (IDs, Metadatas) = sections[i].Storage.Blocks.Convert();

                var chunkOffset = offset;
                IDs                                   .CopyTo(data.Slice( chunkOffset                          + (i * BlockDataLength ), BlockDataLength ));
                Metadatas.Data                        .CopyTo(data.Slice((chunkOffset += i * BlockDataLength)  + (i * NibbleDataLength), NibbleDataLength));
                sections[i].Storage.BlockLight.Data   .CopyTo(data.Slice((chunkOffset += i * NibbleDataLength) + (i * NibbleDataLength), NibbleDataLength));
                sections[i].Storage.BlockSkyLight.Data.CopyTo(data.Slice((chunkOffset += i * NibbleDataLength) + (i * NibbleDataLength), NibbleDataLength));
            }
            chunk.Biomes.CopyTo(data.Slice(offset + (sections.Length * SectionSize), chunk.Biomes.Length));

            offset += (sections.Length * SectionSize) + chunk.Biomes.Length;
        }

        public static ChunkDataPacket CreatePacket(this in Chunk chunk)
        {
            using var rent = MemoryPool<byte>.Shared.Rent(FullChunkSize);
            var buffer = rent.Memory.Span;
            var length = 0;
            chunk.Serialize(in buffer, ref length);

            return new ChunkDataPacket()
            {
                ChunkX = chunk.Location.X,
                ChunkZ = chunk.Location.Z,
                GroundUpContinuous = true,
                PrimaryBitmap = chunk.GetPrimaryBitMap(),
                AddBitmap = 0,
                CompressedData = Compress(buffer.Slice(0, length))
            };
        }

        public static MapChunkBulkPacket MapChunkBulk(this Chunk[] chunks)
        {
            //chunks = chunks.OrderBy(c => c.Coordinates.X).ThenBy(c => c.Coordinates.Z).ToArray();

            using var rent = MemoryPool<byte>.Shared.Rent(FullChunkSize * chunks.Length);
            var buffer = rent.Memory.Span;
            var length = 0;
            foreach (var chunk in chunks)
                chunk.Serialize(in buffer, ref length);

            return new MapChunkBulkPacket()
            {
                Data = Compress(buffer.Slice(0, length)),
                SkyLightSent = true,
                MetaInformation = chunks
                .Select(c => new ChunkColumnMetadata() { PrimaryBitMap = c.GetPrimaryBitMap(), AddBitMap = 0, Coordinates = c.Location })
                .OrderBy(c => c.Coordinates.X).ThenBy(c => c.Coordinates.Z)
                .ToArray(),
            };
        }
    }
}