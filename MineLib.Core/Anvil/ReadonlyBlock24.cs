using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MineLib.Core.Anvil
{
    // ID - 12 bit
    // Meta - 4 bit
    // Light - 4 bit
    // SkyLight - 4 bit
    // Block - 24b
    // Section - 96kb
    // Chunk - 1536kb
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct ReadonlyBlock24 : IBlock, IComparable, IComparable<ReadonlyBlock24>, IEquatable<ReadonlyBlock24>
    {
        public static ReadonlyBlock24 Empty = new ReadonlyBlock24(0, 0, 0, 0);

        public static ReadonlyBlock24 FromIDMeta(ushort idMeta) => new ReadonlyBlock24(idMeta, (byte) 0);
        public static ReadonlyBlock24 FromIDMeta(ushort idMeta, byte skyAndBlockLight) => new ReadonlyBlock24(idMeta, skyAndBlockLight);

        private readonly ushort IDMeta;
        private readonly byte BlockAndSkyLight;

        public uint ID => (IDMeta                           & 0b11111111_11110000u) >> 4;
        public byte Metadata => (byte) ((IDMeta             & 0b00000000_00001111u) >> 0);
        public byte Light => (byte) ((BlockAndSkyLight      & 0b11110000u) >> 4);
        public byte SkyLight => (byte) ((BlockAndSkyLight   & 0b00001111u) >> 0);

        private ReadonlyBlock24(ushort idMeta, byte blockAndSkyLight)
        {
            IDMeta = idMeta;
            BlockAndSkyLight = blockAndSkyLight;
        }

        public ReadonlyBlock24(ulong id)
        {
            IDMeta = (ushort) (
                ((id << 4)          & 0b11111111_11110000u));
            BlockAndSkyLight = 0;
        }

        public ReadonlyBlock24(ulong id, ulong meta)
        {
            IDMeta = (ushort) (
                ((id << 4)          & 0b11111111_11110000u) |
                ((meta << 0)        & 0b00000000_00001111u));
            BlockAndSkyLight = 0;
        }

        public ReadonlyBlock24(ulong id, ulong meta, ulong light)
        {
            IDMeta = (ushort) (
                ((id << 4)          & 0b11111111_11110000u) |
                ((meta << 0)        & 0b00000000_00001111u));
            BlockAndSkyLight = (byte)(
                ((light << 4)       & 0b11110000u));
        }

        public ReadonlyBlock24(ulong id, ulong meta, ulong light, ulong skyLight)
        {
            IDMeta = (ushort) (
                ((id << 4)          & 0b11111111_11110000u) |
                ((meta << 0)        & 0b00000000_00001111u));
            BlockAndSkyLight = (byte)(
                ((light << 4)       & 0b11110000u) |
                ((skyLight << 0)    & 0b00001111u));
        }

        public static bool operator ==(in ReadonlyBlock24 a, in ReadonlyBlock24 b) => a.IDMeta == b.IDMeta && a.BlockAndSkyLight == b.BlockAndSkyLight;
        public static bool operator !=(in ReadonlyBlock24 a, in ReadonlyBlock24 b) => !(a == b);

        public override string ToString() => $"ID: {ID}, Meta: {Metadata}, Light: {Light}, SkyLight: {SkyLight}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is ReadonlyBlock24 block && Equals(block);
        public bool Equals(ReadonlyBlock24 other) => other.IDMeta.Equals(IDMeta) && other.BlockAndSkyLight.Equals(BlockAndSkyLight);

        public override int GetHashCode() => HashCode.Combine(IDMeta, BlockAndSkyLight);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is ReadonlyBlock24 block)
                return CompareTo(block);
            throw new ArgumentException(nameof(obj));
        }
        public int CompareTo(ReadonlyBlock24 other)
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