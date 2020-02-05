using Aragas.Network.Data;
using Aragas.Network.IO;

using fNbt;

using Ionic.Zlib;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Core.Extensions;
using MineLib.Protocol575.Packets.Client.Play;

using System;
using System.Buffers;
using System.Linq;

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
                IDs.CopyTo(data.Slice(chunkOffset + (i * BlockDataLength), BlockDataLength));
                Metadatas.Data.CopyTo(data.Slice((chunkOffset += i * BlockDataLength) + (i * NibbleDataLength), NibbleDataLength));
                sections[i].Storage.BlockLight.Data.CopyTo(data.Slice((chunkOffset += i * NibbleDataLength) + (i * NibbleDataLength), NibbleDataLength));
                sections[i].Storage.BlockSkyLight.Data.CopyTo(data.Slice((chunkOffset += i * NibbleDataLength) + (i * NibbleDataLength), NibbleDataLength));
            }
            chunk.Biomes.CopyTo(data.Slice(offset + (sections.Length * SectionSize), chunk.Biomes.Length));

            offset += (sections.Length * SectionSize) + chunk.Biomes.Length;
        }

        public static uint ConvertToUShort1(Section[] sections)
        {
            uint primaryBitMap = 0, mask = 1;

            mask <<= 1;

            foreach (var section in sections)
            {
                if (!section.IsEmpty)
                    primaryBitMap |= mask;

                mask <<= 1;
            }
            return primaryBitMap;
        }
        public static uint ConvertToUShort2(Section[] sections)
        {
            uint primaryBitMap = 0, mask = 1;

            primaryBitMap |= mask;
            mask <<= 1;

            foreach (var section in sections)
            {
                if (section.IsEmpty)
                    primaryBitMap |= mask;

                mask <<= 1;
            }

            mask <<= 1;
            primaryBitMap |= mask;

            return primaryBitMap;
        }
        public static UpdateLightPacket CreateLightPacket(this in Chunk chunk)
        {
            var sl = ConvertToUShort1(chunk.Sections);

            var sl2 = ConvertToUShort2(chunk.Sections);

            return new UpdateLightPacket()
            {
                X = new VarInt(chunk.Location.X),
                Z = new VarInt(chunk.Location.Z),

                SkyLightMask = sl,
                BlockLightMask = sl,
                EmptySkyLightMask = sl2,
                EmptyBlockLightMask = sl2,

                Data = chunk.GetData1().ToArray()
            };
        }
        private static Span<byte> GetData1(this in Chunk chunk)
        {
            var count = chunk.Sections.Where(s => s.IsEmpty).Count();
            //var adata = new byte[2048 * 2 * count + new VarInt(2048).Size * count];
            //int offset = 0;
            var serializer = new ProtobufSerializer();
            foreach (var section in chunk.Sections.Where(s => !s.IsEmpty))
            {
                serializer.Write(section.Storage.BlockSkyLight.Data);
                serializer.Write(section.Storage.BlockLight.Data);
            }
            return serializer.GetData();
        }

        public static ChunkDataPacket CreatePacket(this in Chunk chunk)
        {
            return new ChunkDataPacket()
            {
                X = chunk.Location.X,
                Z = chunk.Location.Z,
                GroundUp = true,
                BitMap = new VarInt(chunk.GetPrimaryBitMap()),
                DataLength = chunk.GetDataSize(),
                ChunkData = chunk.GetData().ToArray(),
                Biomes = chunk.Biomes.Select(b => (int) b).ToArray(),

                Nbt = chunk.GetHeightMap()
            };
        }

        private static int GetDataSize(this in Chunk chunk)
        {
            int data_size = 1024; // account for biome array
            foreach (var section in chunk.Sections.Where(s => !s.IsEmpty))
            {
                var bits_per_block = section.Storage.Blocks.BitsPerBlock;
                data_size += 2; // number of non-air blocks
                data_size += 1; // bits per block

                if (bits_per_block <= 8)
                {
                    /*
                    data_size += new VarLong(palette.num_blocks()).Size;
                    for (unsigned short id : palette.ids)
                        data_size += varlong_size(id);
                    */
                }

                data_size += new VarLong(bits_per_block * 512).Size; // data array length
                data_size += bits_per_block * 512; // data array
            }
            return data_size;
        }
        private static Span<byte> GetData(this in Chunk chunk)
        {
            using var serializer = new ProtobufSerializer();
            foreach (var section in chunk.Sections.Where(s => !s.IsEmpty))
            {
                serializer.Write((short) section.count_non_air_blocks());

                var bits_per_block = section.Storage.Blocks.BitsPerBlock;
                serializer.Write((byte) bits_per_block);

                if (bits_per_block <= 8)
                {
                    /*
                    // indirect mode
                    writer.write_varlong(palette.num_blocks());
                    for (auto id : palette.ids)
                        writer.write_varlong(id);
                    */
                }

                serializer.Write(section.Storage.Blocks.Data);
            }
            return serializer.GetData();
        }

        private static NbtCompound GetHeightMap(this in Chunk chunk) => new fNbt.NbtCompound("") { new NbtLongArray("MOTION_BLOCKING", compute_height_map(in chunk)) };
        private static long[] compute_height_map(in Chunk chunk)
        {
            var height_map = new long[256];
            for (var x = 0; x < 16; ++x)
            {
                for (var z = 0; z < 16; ++z)
                {
                    height_map[(x * 16) + z] = -1;
                    for (var y = 255; y >= 0; --y)
                    {
                        if (!is_transparent_block(chunk.GetBlock(new Location3D(x, y, z))))
                        {
                            height_map[(x * 16) + z] = y;
                            break;
                        }
                    }
                }
            }
            return height_map;
        }
        private static bool is_transparent_block(in ReadonlyBlock32 block)
        {
            return block.ID == 0;
        }

        private static int count_non_air_blocks(this in Section section)
        {
            int count = 0;
            foreach (var (ID, Metadata) in section.Storage.Blocks)
                if (ID != 0)
                    ++count;
            return count;

        }
    }
}