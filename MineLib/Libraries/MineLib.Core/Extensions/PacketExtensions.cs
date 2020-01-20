using Aragas.Network.Data;
using Aragas.Network.IO;

using MineLib.Core.Anvil;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

using static Aragas.Network.IO.PacketSerializer;
using static Aragas.Network.IO.PacketDeserializer;

namespace MineLib.Core.Extensions
{
    public static class PacketExtensions
    {
        private static void Extend<T>(Func<PacketDeserializer, int, T> readFunc, Action<PacketSerializer, T, bool> writeAction)
        {
            ExtendRead(readFunc);
            ExtendWrite(writeAction);
        }

        public static void Init()
        {
            Extend<ulong[]>(ReadUInt64Array, WriteUInt64Array);
            Extend<Location2D>(ReadLocation2D, WriteLocation2D);
            Extend<Location3D>(ReadLocation3D, WriteLocation3D);
            Extend<Chunk>(ReadChunk, WriteChunk);
            Extend<Chunk[]>(ReadChunkArray, WriteChunkArray);
            Extend<Section[]>(ReadSectionArray, WriteSectionArray);
            Extend<Section>(ReadSection, WriteSection);
            Extend<BlockListWithLight>(ReadBlockListWithLight, WriteBlockListWithLight);
            Extend<BlockStorage64>(ReadBlockStorage64, WriteBlockStorage64);
            Extend<NibbleArray>(ReadNibbleArray, WriteNibbleArray);
            Extend<Vector3>(ReadVector3, WriteVector3);
            Extend<Look>(ReadLook, WriteLook);
            Extend<Rotation>(ReadRotation, WriteRotation);
            Extend<IPlayer>(ReadIPlayer, WriteIPlayer);
        }
        private static void WriteUInt64Array(PacketSerializer serializer, ulong[] value, bool writeDefaultLength = true)
        {
            serializer.Write(MemoryMarshal.Cast<ulong, byte>(value).ToArray(), writeDefaultLength);
            //serializer.Write(ZlibStream.CompressBuffer(MemoryMarshal.Cast<ulong, byte>(value).ToArray()), writeDefaultLength);
        }
        private static ulong[] ReadUInt64Array(PacketDeserializer deserializer, int length = 0)
        {
            return MemoryMarshal.Cast<byte, ulong>(deserializer.Read<byte[]>(Array.Empty<byte>(), length)).ToArray();
            //return MemoryMarshal.Cast<byte, ulong>(ZlibStream.UncompressBuffer(deserializer.Read<byte[]>(null, length))).ToArray();
        }

        private static void WriteLocation2D(PacketSerializer serializer, Location2D value, bool writeDefaultLength = true)
        {
            serializer.Write(value.X);
            serializer.Write(value.Z);
        }
        private static Location2D ReadLocation2D(PacketDeserializer deserializer, int length = 0)
        {
            return new Location2D(deserializer.Read<int>(), deserializer.Read<int>());
        }

        private static void WriteLocation3D(PacketSerializer serializer, Location3D value, bool writeDefaultLength = true)
        {
            serializer.Write(value.X);
            serializer.Write(value.Y);
            serializer.Write(value.Z);
        }
        private static Location3D ReadLocation3D(PacketDeserializer deserializer, int length = 0)
        {
            return new Location3D(deserializer.Read<int>(), deserializer.Read<int>(), deserializer.Read<int>());
        }

        private static void WriteChunk(PacketSerializer serializer, Chunk value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Location);
            serializer.Write(value.Biomes);
            serializer.Write(value.Sections);
        }
        private static Chunk ReadChunk(PacketDeserializer deserializer, int length = 0)
        {
            var chunk = new Chunk(deserializer.Read<Location2D>());
            var biomes = deserializer.Read<byte[]>();
            var sections = deserializer.Read<Section[]>();
            for (int i = 0; i < chunk.Biomes.Length; i++)
                chunk.Biomes[i] = biomes[i];
            for (int i = 0; i < chunk.Sections.Length; i++)
                chunk.Sections[i] = sections[i];
            return chunk;
        }

        private static void WriteChunkArray(PacketSerializer serializer, Chunk[] value, bool writeDefaultLength = true)
        {
            if (writeDefaultLength)
                serializer.Write(new VarInt(value.Length));
            foreach (var chunk in value)
                serializer.Write(chunk);
        }
        private static Chunk[] ReadChunkArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();
            var list = new List<Chunk>();
            for (int i = 0; i < length; i++)
                list.Add(deserializer.Read<Chunk>());
            return list.ToArray();
        }

        private static void WriteSectionArray(PacketSerializer serializer, Section[] value, bool writeDefaultLength = true)
        {
            if (writeDefaultLength)
                serializer.Write(new VarInt(value.Length));
            foreach (var section in value)
                serializer.Write(section);
        }
        private static Section[] ReadSectionArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();
            var list = new List<Section>();
            for (int i = 0; i < length; i++)
                list.Add(deserializer.Read<Section>());
            return list.ToArray();
        }

        private static void WriteSection(PacketSerializer serializer, Section value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Location);
            serializer.Write(value.IsEmpty);
            if(!value.IsEmpty)
                serializer.Write(value.Storage);
        }
        private static Section ReadSection(PacketDeserializer deserializer, int length = 0)
        {
            var location = deserializer.Read<Location3D>();
            var isEmpty = deserializer.Read<bool>();
            if (!isEmpty)
            {
                var blocks = deserializer.Read<BlockListWithLight>();
                return new Section(location, blocks);
            }
            return Section.BuildEmpty(location);
        }

        private static void WriteBlockListWithLight(PacketSerializer serializer, BlockListWithLight value, bool writeDefaultLength = true)
        {
            serializer.Write(value.XSize);
            serializer.Write(value.YSize);
            serializer.Write(value.ZSize);

            serializer.Write(value.Blocks);
            serializer.Write(value.BlockLight);
            serializer.Write(value.BlockSkyLight);
        }
        private static BlockListWithLight ReadBlockListWithLight(PacketDeserializer deserializer, int length = 0)
        {
            return new BlockListWithLight(
                deserializer.Read<int>(),
                deserializer.Read<int>(),
                deserializer.Read<int>(),
                deserializer.Read<BlockStorage64>(),
                deserializer.Read<NibbleArray>(),
                deserializer.Read<NibbleArray>());
        }

        private static void WriteBlockStorage64(PacketSerializer serializer, BlockStorage64 value, bool writeDefaultLength = true)
        {
            serializer.Write(value.BitsPerBlock);
            serializer.Write(value.Data);
            serializer.Write(value.BitsPerMetadata);
        }
        private static BlockStorage64 ReadBlockStorage64(PacketDeserializer deserializer, int length = 0)
        {
            return new BlockStorage64(
                deserializer.Read<byte>(),
                deserializer.Read<ulong[]>(),
                deserializer.Read<byte>());
        }

        private static void WriteNibbleArray(PacketSerializer serializer, NibbleArray value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Data);
            //serializer.Write(ZlibStream.CompressBuffer(value.Data));
        }
        private static NibbleArray ReadNibbleArray(PacketDeserializer deserializer, int length = 0)
        {
            return new NibbleArray(deserializer.Read<byte[]>());
            //return new NibbleArray(ZlibStream.UncompressBuffer(deserializer.Read<byte[]>()));
        }


        private static void WriteVector3(PacketSerializer serializer, Vector3 value, bool writeDefaultLength = true)
        {
            serializer.Write(value.X);
            serializer.Write(value.Y);
            serializer.Write(value.Z);
        }
        private static Vector3 ReadVector3(PacketDeserializer deserializer, int length = 0)
        {
            return new Vector3(deserializer.Read<float>(), deserializer.Read<float>(), deserializer.Read<float>());
        }


        private static void WriteLook(PacketSerializer serializer, Look value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Pitch);
            serializer.Write(value.Yaw);
        }
        private static Look ReadLook(PacketDeserializer deserializer, int length = 0)
        {
            return new Look(deserializer.Read<float>(), deserializer.Read<float>());
        }

        private static void WriteRotation(PacketSerializer serializer, Rotation value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Pitch);
            serializer.Write(value.Yaw);
            serializer.Write(value.Roll);
        }
        private static Rotation ReadRotation(PacketDeserializer deserializer, int length = 0)
        {
            return new Rotation(deserializer.Read<float>(), deserializer.Read<float>(), deserializer.Read<float>());
        }

        private static void WriteIPlayer(PacketSerializer serializer, IPlayer value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Username);
            serializer.Write(value.Uuid);
            serializer.Write(value.Position);
            serializer.Write(value.Look);
        }
        private static IPlayer ReadIPlayer(PacketDeserializer deserializer, int length = 0)
        {
            return new Player()
            {
                Username = deserializer.Read<string>(),
                Uuid = deserializer.Read<Guid>(),
                Position = deserializer.Read<Vector3>(),
                Look = deserializer.Read<Look>(),
            };
        }
    }
}