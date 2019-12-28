using Aragas.Network.Data;
using Aragas.Network.IO;

using fNbt;

using MineLib.Core;
using MineLib.Protocol5.Data;

using System;

using static Aragas.Network.IO.PacketSerializer;
using static Aragas.Network.IO.PacketDeserializer;

namespace MineLib.Protocol5.Extensions
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
            Extend<ChunkColumnMetadata[]>(ReadChunkColumnMetadataArray, WriteChunkColumnMetadataArray);
            Extend<ItemSlot>(ReadItemStack, WriteItemStack);
        }

        private static void WriteChunkColumnMetadataArray(PacketSerializer serializer, ChunkColumnMetadata[] value, bool writeDefaultLength = true)
        {
            if (writeDefaultLength)
                serializer.Write(new VarInt(value.Length));

            foreach (var entry in value)
            {
                serializer.Write(entry.Coordinates.X);
                serializer.Write(entry.Coordinates.Z);
                serializer.Write(entry.PrimaryBitMap);
                serializer.Write(entry.AddBitMap); // AddBitmap
            }
        }
        private static ChunkColumnMetadata[] ReadChunkColumnMetadataArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();

            var array = new ChunkColumnMetadata[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = new ChunkColumnMetadata
                {
                    Coordinates = new Location2D(deserializer.Read<int>(), deserializer.Read<int>()),
                    PrimaryBitMap = deserializer.Read<ushort>(),
                    AddBitMap = deserializer.Read<ushort>()
                };

            }

            return array;
        }

        private static void WriteItemStack(PacketSerializer serializer, ItemSlot value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Id);
            if (value.IsEmpty) return;
            serializer.Write(value.Count);
            serializer.Write(value.GetMetadata());
            if (value.Nbt == null)
            {
                serializer.Write<short>(-1);
                return;
            }

            var file = new NbtFile(value.Nbt);
            var compressed = file.SaveToBuffer(NbtCompression.GZip);
            serializer.Write((short) compressed.Length);
            serializer.Write(compressed, false);
        }
        private static ItemSlot ReadItemStack(PacketDeserializer deserializer, int length = 0)
        {
            var id = deserializer.Read<short>();
            if (id == -1) return ItemSlot.Empty;
            var count = deserializer.Read<sbyte>();
            var metadata = deserializer.Read<short>();
            var nbtLength = deserializer.Read<short>();
            if (nbtLength == -1)
                return new ItemSlot(id, count);

            var compressed = deserializer.Read<byte[]>(null, nbtLength);
            var file = new NbtFile();
            file.LoadFromBuffer(compressed, 0, compressed.Length, NbtCompression.AutoDetect);
            var nbt = file.RootTag;

            var slot = new ItemSlot(id, count, nbt);
            slot.SetMetadata(metadata);
            return slot;
        }
    }
}