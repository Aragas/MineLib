using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MineLib.Core.Anvil
{
    // ID - 20 bit
    // Meta - 4 bit
    // Light - 4 bit
    // SkyLight - 4 bit
    // Block - 32b
    // Section - 128kb
    // Chunk - 2048kb
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct ReadonlyBlock32 : IBlock, IComparable, IComparable<ReadonlyBlock32>, IEquatable<ReadonlyBlock32>
    {
        public static ReadonlyBlock32 Empty = new ReadonlyBlock32(0, 0, 0, 0);

        public static ReadonlyBlock32 FromIDMeta(ushort idMeta) => new ReadonlyBlock32(idMeta, (byte) 0);
        public static ReadonlyBlock32 FromIDMeta(ushort idMeta, byte blockAndSkyLight) => new ReadonlyBlock32(idMeta, blockAndSkyLight);

        private readonly uint IDMetaSkyAndBlockLight;

        public uint ID => (IDMetaSkyAndBlockLight               & 0b11111111_11111111_11110000_00000000u) >> 12;
        public byte Metadata => (byte) ((IDMetaSkyAndBlockLight & 0b00000000_00000000_00001111_00000000u) >> 08);
        public byte Light => (byte) ((IDMetaSkyAndBlockLight    & 0b00000000_00000000_00000000_11110000u) >> 04);
        public byte SkyLight => (byte) ((IDMetaSkyAndBlockLight & 0b00000000_00000000_00000000_00001111u) >> 00);

        private ReadonlyBlock32(ushort idMeta, byte blockAndSkyLight)
        {
            IDMetaSkyAndBlockLight = (uint) (
                ((idMeta << 8)           & 0b11111111_11111111_11111111_00000000u) |
                ((blockAndSkyLight << 0) & 0b00000000_00000000_00000000_11111111u));
        }

        public ReadonlyBlock32(ulong id)
        {
            IDMetaSkyAndBlockLight = (uint) (
                ((id << 12)       & 0b11111111_11111111_11110000_00000000u));
        }

        public ReadonlyBlock32(ulong id, ulong meta)
        {
            IDMetaSkyAndBlockLight = (uint) (
                ((id << 12)      & 0b11111111_11111111_11110000_00000000u) |
                ((meta << 8)     & 0b00000000_00000000_00001111_00000000u));
        }

        public ReadonlyBlock32(ulong id, ulong meta, ulong light)
        {
            IDMetaSkyAndBlockLight = (uint) (
                ((id << 12) & 0b11111111_11111111_11110000_00000000u) |
                ((meta << 8)    & 0b00000000_00000000_00001111_00000000u) |
                ((light << 4)   & 0b00000000_00000000_00000000_11110000u));
        }

        public ReadonlyBlock32(ulong id, ulong meta, ulong light, ulong skyLight)
        {
            IDMetaSkyAndBlockLight = (uint) (
                ((id << 12)      & 0b11111111_11111111_11110000_00000000u) |
                ((meta << 8)     & 0b00000000_00000000_00001111_00000000u) |
                ((light << 4)    & 0b00000000_00000000_00000000_11110000u) |
                ((skyLight << 0) & 0b00000000_00000000_00000000_00001111u));
    }

        public static bool operator ==(in ReadonlyBlock32 a, in ReadonlyBlock32 b) => a.IDMetaSkyAndBlockLight == b.IDMetaSkyAndBlockLight;
        public static bool operator !=(in ReadonlyBlock32 a, in ReadonlyBlock32 b) => !(a == b);

        public override string ToString() => $"ID: {ID}, Meta: {Metadata}, Light: {Light}, SkyLight: {SkyLight}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is ReadonlyBlock32 block && Equals(block);
        public bool Equals(ReadonlyBlock32 other) => other.IDMetaSkyAndBlockLight.Equals(IDMetaSkyAndBlockLight);

        public override int GetHashCode() => IDMetaSkyAndBlockLight.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is ReadonlyBlock32 block)
                return CompareTo(block);
            throw new ArgumentException(nameof(obj));
        }
        public int CompareTo(ReadonlyBlock32 other)
        {
            if (ID > other.ID) return 1;
            if (ID < other.ID) return -1;
            if (ID == other.ID)
            {
                if (Metadata > other.Metadata) return 1;
                if (Metadata < other.Metadata) return -1;
            }
            return 0;
        }
    }
}