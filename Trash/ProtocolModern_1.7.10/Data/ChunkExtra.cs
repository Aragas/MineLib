using System;
using System.Collections.Generic;

using MineLib.Core.Data.Anvil;

namespace ProtocolModern.Data
{
    public static class ChunkExtra
    {
        public static Chunk[] ReadChunkArray(bool skyLight, ChunkColumnMetadata[] metadata, byte[] data)
        {
            var list = new List<Chunk>();

            var offset = 0;
            foreach (var meta in metadata)
            {
                var chunkLength = (Chunk.OneByteData + (Chunk.HalfByteData * 2) + (skyLight ? Chunk.HalfByteData : 0)) *
                    Chunk.GetSectionCount(meta.PrimaryBitMap) +
                        Chunk.HalfByteData * Chunk.GetSectionCount(meta.AddBitMap) + (Chunk.Width * Chunk.Depth);

                var chunkData = new byte[chunkLength];
                Array.Copy(data, offset, chunkData, 0, chunkLength);
                list.Add(ReadChunk(skyLight, true, meta, chunkData));

                offset += chunkLength;
            }

            return list.ToArray();
        }
        public static Chunk ReadChunk(bool lightIncluded, bool groundUp, ChunkColumnMetadata metadata, byte[] data)
        {
            var chunk = new Chunk(metadata.Coordinates);
            var sectionCount = Chunk.GetSectionCount(metadata.PrimaryBitMap);

            for (int y = 0; y < 16; y++)
            {
                if ((metadata.PrimaryBitMap & (1 << y)) > 0)
                {
                    var rawBlocks = new byte[Chunk.OneByteData];
                    var rawBlocksMetadata = new byte[Chunk.HalfByteData];
                    var rawBlocksLight = new byte[Chunk.HalfByteData];
                    var rawSkylight = new byte[Chunk.HalfByteData];

                    // Blocks
                    Array.Copy(data, y * Chunk.OneByteData, rawBlocks, 0, Chunk.OneByteData);
                    // Metadata
                    Array.Copy(data, (Chunk.OneByteData * sectionCount) + (y * Chunk.HalfByteData),
                        rawBlocksMetadata, 0, Chunk.HalfByteData);
                    // Light
                    Array.Copy(data, ((Chunk.OneByteData + Chunk.HalfByteData) * sectionCount) + (y * Chunk.HalfByteData),
                        rawBlocksLight, 0, Chunk.HalfByteData);
                    // Sky light
                    if (lightIncluded)
                        Array.Copy(data, ((Chunk.OneByteData + Chunk.HalfByteData + Chunk.HalfByteData) * sectionCount) + (y * Chunk.HalfByteData),
                            rawSkylight, 0, Chunk.HalfByteData);

                    chunk.Sections[y].BuildFromNibbleData(CombineBlockMetadata(rawBlocks, Section.ToBytePerBlock(rawBlocksMetadata)), rawBlocksLight, rawSkylight);
                }
            }
            if (groundUp)
                Array.Copy(data, data.Length - chunk.Biomes.Length, chunk.Biomes, 0, chunk.Biomes.Length);

            return chunk;
        }

        private static byte[] CombineBlockMetadata(byte[] block, byte[] metadata)
        {
            var array = new ushort[Chunk.OneByteData];
            for (var i = 0; i < array.Length; i++)
                array[i] = (ushort)(block[i] << 4 & 0xFFF0 | metadata[i] & 0x000F);

            var arr = new byte[Chunk.TwoByteData];
            Buffer.BlockCopy(array, 0, arr, 0, arr.Length);

            return arr;
        }
    }
}
