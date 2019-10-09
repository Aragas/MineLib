using System;
using System.Collections.Generic;

using Aragas.Core.Data;
using Aragas.Core.IO;

using fNbt;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Exceptions;

using Org.BouncyCastle.Math;

using ProtocolModern.Data;

using static Aragas.Core.IO.PacketStream;
using static Aragas.Core.IO.PacketDataReader;

namespace ProtocolModern.Extensions
{
    public static class PacketExtensions
    {
        private static void Extend<T>(Func<PacketDataReader, int, T> readFunc, Action<PacketStream, T, bool> writeAction)
        {
            ExtendRead(readFunc);
            ExtendWrite(writeAction);
        }

        public static void Init()
        {
            Extend<Chunk>(ReadChunk, WriteChunk);

            Extend<Chunk[]>(ReadChunkArray, WriteChunkArray);

            Extend<PlayerData[]>(ReadPlayerDataArray, WritePlayerDataArray);

            Extend<ChunkColumnMetadata[]>(ReadChunkColumnMetadataArray, WriteChunkColumnMetadataArray);

            Extend<EntityProperty[]>(ReadEntityPropertyArray, WriteEntityPropertyArray);

            Extend<EntityMetadataList>(ReadEntityMetadata, WriteEntityMetadataList);

            Extend<StatisticsEntry[]>(ReadStatisticsEntryArray, WriteStatisticsEntryArray);

            Extend<ItemStack>(ReadItemStack, WriteItemStack);

            Extend<ItemStack[]>(ReadItemStackArray, WriteItemStackArray);

            Extend<BlockPosition[]>(ReadBlockPositionArray, WriteBlockPositionArray);

            Extend<BigInteger>(ReadBigInteger, WriteBigInteger);
        }

        public static void WriteChunk(PacketStream stream, Chunk value, bool writeDefaultLength = true) { }
        private static Chunk ReadChunk(PacketDataReader reader, int length = 0)
        {
            var chunk = new Chunk(new Coordinates2D(reader.Read<int>(), reader.Read<int>()));
            //chunk.Coordinates = new Coordinates2D(reader.Read<int>(), reader.Read<int>());
            chunk.GroundUp = reader.Read<bool>();
            var primaryBitMap = reader.Read<ushort>();
            chunk.OverWorld = true; // TODO: From World class

            var size = reader.Read<VarInt>();
            var data = reader.Read<byte[]>(null, size);

            var sectionCount = Chunk.GetSectionCount(primaryBitMap);

            var chunkRawBlocks      = new byte[sectionCount * Chunk.TwoByteData];
            var chunkRawBlocksLight = new byte[sectionCount * Chunk.HalfByteData];
            var chunkRawSkylight    = new byte[sectionCount * Chunk.HalfByteData];

            Buffer.BlockCopy(data, 0,                                                     chunkRawBlocks,         0, chunkRawBlocks.Length * sizeof(byte)       );
            Buffer.BlockCopy(data, chunkRawBlocks.Length,                                 chunkRawBlocksLight,    0, chunkRawBlocksLight.Length * sizeof(byte)  );
            Buffer.BlockCopy(data, chunkRawBlocks.Length + chunkRawBlocksLight.Length,    chunkRawSkylight,       0, chunkRawSkylight.Length * sizeof(byte)     );

            for (int y = 0, i = 0; y < 16; y++)
            {
                if ((primaryBitMap & (1 << y)) > 0)
                {
                    // Blocks & Metadata
                    var rawBlocks = new byte[Chunk.TwoByteData];
                    Buffer.BlockCopy(chunkRawBlocks, i * rawBlocks.Length, rawBlocks, 0, rawBlocks.Length * sizeof(byte));

                    // Light, convert to 1 byte per block
                    var rawBlockLight = new byte[Chunk.HalfByteData];
                    Buffer.BlockCopy(chunkRawBlocksLight, i * rawBlockLight.Length, rawBlockLight, 0, rawBlockLight.Length * sizeof(byte));

                    // Sky light, convert to 1 byte per block
                    var rawSkyLight = new byte[Chunk.HalfByteData];
                    if (chunk.OverWorld)
                        Buffer.BlockCopy(chunkRawSkylight, i * rawSkyLight.Length, rawSkyLight, 0, rawSkyLight.Length * sizeof(byte));

                    chunk.Sections[y].BuildFromNibbleData(rawBlocks, rawBlockLight, rawSkyLight);
                    i++;
                }
            }
            if (chunk.GroundUp)
                Buffer.BlockCopy(data, data.Length - chunk.Biomes.Length, chunk.Biomes, 0, chunk.Biomes.Length * sizeof(byte));

            return chunk;
        }

        public static void WriteChunkArray(PacketStream stream, Chunk[] chunkList, bool writeDefaultLength = true) { }
        private static Chunk[] ReadChunkArray(PacketDataReader reader, int length = 0)
        {
            var groundUp = reader.Read<bool>();

            var count = reader.Read<VarInt>();
            var metadata = reader.Read<ChunkColumnMetadata[]>(null, count);
            //var metadata = ChunkColumnMetadataList.FromReader(reader);

            int totalSections = 0;
            foreach (var meta in metadata)
                totalSections += Chunk.GetSectionCount(meta.PrimaryBitMap);


            var size = totalSections * (Chunk.TwoByteData + Chunk.HalfByteData + (groundUp ? Chunk.HalfByteData : 0)) + metadata.Length * Chunk.BiomesLength;
            var data = reader.Read<byte[]>(null, size);

            var chunks = new List<Chunk>();
            int offset = 0;
            foreach (var meta in metadata)
            {
                var chunk = new Chunk(meta.Coordinates);
                chunk.OverWorld = true;
                var primaryBitMap = meta.PrimaryBitMap;

                var sectionCount = Chunk.GetSectionCount(primaryBitMap);

                var chunkRawBlocks = new byte[sectionCount * Chunk.TwoByteData];
                var chunkRawBlocksLight = new byte[sectionCount * Chunk.HalfByteData];
                var chunkRawSkylight = new byte[sectionCount * Chunk.HalfByteData];

                var chunkLength = sectionCount * (Chunk.TwoByteData + Chunk.HalfByteData + (chunk.OverWorld ? Chunk.HalfByteData : 0)) + Chunk.BiomesLength;
                var chunkData = new byte[chunkLength];
                Buffer.BlockCopy(data, offset, chunkData, 0, chunkData.Length * sizeof(byte));

                Buffer.BlockCopy(chunkData, 0, chunkRawBlocks, 0, chunkRawBlocks.Length * sizeof(byte));
                Buffer.BlockCopy(chunkData, chunkRawBlocks.Length, chunkRawBlocksLight, 0, chunkRawBlocksLight.Length * sizeof(byte));
                Buffer.BlockCopy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length, chunkRawSkylight, 0, chunkRawSkylight.Length * sizeof(byte));
                if (groundUp)
                    Buffer.BlockCopy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length + chunkRawSkylight.Length, chunk.Biomes, 0, Chunk.BiomesLength * sizeof(byte));

                for (int y = 0, i = 0; y < 16; y++)
                {
                    if ((primaryBitMap & (1 << y)) > 0)
                    {
                        // Blocks & Metadata
                        var rawBlocks = new byte[Chunk.TwoByteData];
                        Buffer.BlockCopy(chunkRawBlocks, i * rawBlocks.Length, rawBlocks, 0, rawBlocks.Length * sizeof(byte));

                        // Light
                        var rawBlockLight = new byte[Chunk.HalfByteData];
                        Buffer.BlockCopy(chunkRawBlocksLight, i * rawBlockLight.Length, rawBlockLight, 0, rawBlockLight.Length * sizeof(byte));

                        // Sky light
                        var rawSkyLight = new byte[Chunk.HalfByteData];
                        if (chunk.OverWorld)
                            Buffer.BlockCopy(chunkRawSkylight, i * rawSkyLight.Length, rawSkyLight, 0, rawSkyLight.Length * sizeof(byte));

                        chunk.Sections[y].BuildFromNibbleData(rawBlocks, rawBlockLight, rawSkyLight);
                        i++;
                    }
                }
                chunks.Add(chunk);

                offset += chunkLength;
            }

            if (offset != data.Length)
                throw new NetworkHandlerException("Map Chunk Bulk reading error: offset != data.Length");

            return chunks.ToArray();
        }

        public static void WritePlayerDataArray(PacketStream stream, PlayerData[] value, bool writeDefaultLength = true) { }
        private static PlayerData[] ReadPlayerDataArray(PacketDataReader reader, int length = 0)
        {
            return null;
        }

        private static void WriteChunkColumnMetadataArray(PacketStream stream, ChunkColumnMetadata[] value, bool writeDefaultLength = true)
        {
            if (writeDefaultLength)
                stream.Write(new VarInt(value.Length));

            foreach (var entry in value)
            {
                stream.Write(entry.Coordinates.X);
                stream.Write(entry.Coordinates.Z);
                stream.Write(entry.PrimaryBitMap);
                stream.Write((short) 0); // AddBitmap
            }
        }
        private static ChunkColumnMetadata[] ReadChunkColumnMetadataArray(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length = reader.Read<VarInt>();

            var array = new ChunkColumnMetadata[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = new ChunkColumnMetadata
                {
                    Coordinates = new Coordinates2D(reader.Read<int>(), reader.Read<int>()),
                    PrimaryBitMap = reader.Read<ushort>(),
                    AddBitMap = reader.Read<ushort>()
            };
                
            }

            return array;
        }

        private static void WriteEntityPropertyArray(PacketStream stream, EntityProperty[] value, bool writeDefaultLength = true)
        {
            if(writeDefaultLength)
                stream.Write(value.Length);

            foreach (var entry in value)
            {
                stream.Write(entry.Key);
                stream.Write(entry.Value);

                stream.Write((short) entry.Modifiers.Length);
                foreach (var modifiers in entry.Modifiers)
                {
                    stream.Write(modifiers.UUID);
                    stream.Write(modifiers.Amount);
                    stream.Write(modifiers.Operation);
                }
            }
        }
        private static EntityProperty[] ReadEntityPropertyArray(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length = reader.Read<int>();

            var array = new EntityProperty[length];
            for (var i = 0; i < length; i++)
            {
                var property = new EntityProperty();

                property.Key = reader.Read<string>();
                property.Value = reader.Read<double>();

                var listLength = reader.Read<short>();
                property.Modifiers = new Modifiers[listLength];
                for (var j = 0; j < listLength; j++)
                    property.Modifiers[j] = new Modifiers
                    {
                        UUID = reader.Read<BigInteger>(),
                        Amount = reader.Read<double>(),
                        Operation = reader.Read<sbyte>()
                    };
                

                array[i] = property;
            }

            return array;
        }

        private static void WriteEntityMetadataList(PacketStream stream, EntityMetadataList value, bool writeDefaultLength = true)
        {
            foreach (var entry in value._entries)
                entry.Value.ToStream(stream, entry.Key);

            stream.Write((byte)0x7F);
        }
        private static EntityMetadataList ReadEntityMetadata(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length++;

            var array = new EntityMetadataList(length);

            while (true)
            {
                byte key = reader.Read<byte>();
                if (key == 127) break;

                var type = (byte)((key & 0xE0) >> 5);
                var index = (byte)(key & 0x1F);

                var entry = EntityMetadataList.EntryTypes[type]();
                entry.FromReader(reader);
                entry.Index = index;

                array[index] = entry;
            }
            return array;
        }

        private static void WriteStatisticsEntryArray(PacketStream stream, StatisticsEntry[] value, bool writeDefaultLength = true) { }
        private static StatisticsEntry[] ReadStatisticsEntryArray(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length = reader.Read<VarInt>();

            var array = new StatisticsEntry[length];
            for (var i = 0; i < array.Length; i++)
                array[i] = new StatisticsEntry
                {
                    StatisticsName = reader.Read<string>(),
                    Value = reader.Read<VarInt>()
                };
            return array;
        }

        private static void WriteItemStack(PacketStream stream, ItemStack value, bool writeDefaultLength = true)
        {
            stream.Write(value.ID);
            if (value.Empty)
                return;

            stream.Write((byte) value.Count);
            stream.Write(value.Damage);
            if (value.Nbt == null)
            {
                stream.Write((short) -1);
                return;
            }

            var file = new NbtFile(value.Nbt);
            file.SaveToProtocolStream(stream, NbtCompression.GZip);
        }
        private static ItemStack ReadItemStack(PacketDataReader reader, int length = 0)
        {
            var itemStack = new ItemStack(reader.Read<short>());

            if (itemStack.Empty)
                return itemStack;

            itemStack.Count = reader.Read<byte>();
            itemStack.Damage = reader.Read<short>();

            var buffLength = reader.Read<short>();
            if (buffLength == -1 || buffLength == 0)
                return itemStack;

            itemStack.Nbt = new NbtCompound();
            var buffer = reader.Read<byte[]>(null, buffLength);
            var nbt = new NbtFile();
            nbt.LoadFromBuffer(buffer, 0, buffLength, NbtCompression.GZip, null);
            itemStack.Nbt = nbt.RootTag;

            return itemStack;
        }

        private static void WriteItemStackArray(PacketStream stream, ItemStack[] value, bool writeDefaultLength = true)
        {
            if (writeDefaultLength)
                stream.Write((short) value.Length);

            foreach (var itemStack in value)
            {
                //if (itemStack.ID == 1) // AIR
                //    return;

                stream.Write(itemStack.ID);
                stream.Write(itemStack.Damage);
                stream.Write((short)itemStack.Count);

                //if (itemStack.Empty)
                //    stream.WriteSByte(itemStack.Slot);

                if (itemStack.Nbt == null)
                {
                    stream.Write((short)-1);
                    return;
                }
            }
        }
        private static ItemStack[] ReadItemStackArray(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length = reader.Read<short>();

            var array = new ItemStack[length];

            for (int i = 0; i < length; i++)
                array[i] = reader.Read<ItemStack>();

            return array;
        }

        private static void WriteBlockPositionArray(PacketStream stream, BlockPosition[] value, bool writeDefaultLength = true)
        {
            if (writeDefaultLength)
                stream.Write(value.Length);

            foreach (var entry in value)
            {
            }
        }
        private static BlockPosition[] ReadBlockPositionArray(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length = reader.Read<int>();

            var array = new BlockPosition[length];
            for (var i = 0; i < length; i++)
            {
                var coordinates = reader.Read<short>();
                var y = coordinates & 0xFF;
                var z = (coordinates >> 8) & 0xf;
                var x = (coordinates >> 12) & 0xf;

                var blockIDMeta = reader.Read<VarInt>();

                array[i] = new BlockPosition((ushort) blockIDMeta, new Position(x, y, z));
            }

            return array;
        }

        private static void WriteBigInteger(PacketStream stream, BigInteger value, bool writeDefaultLength = true)
        {
            stream.Write(value.ToByteArray());
        }
        private static BigInteger ReadBigInteger(PacketDataReader reader, int length = 0)
        {
            return new BigInteger(reader.Read<byte[]>(null, 4));
        }
    }
}
