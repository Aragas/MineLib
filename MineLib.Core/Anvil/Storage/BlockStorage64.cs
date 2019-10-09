using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MineLib.Core.Anvil
{
    //public class BlockMeta(ulong ID, ulong Metadata);


    // 14 bit - 56kb
    public readonly struct BlockStorage64 : IEnumerable<(ulong ID, ulong Metadata)>
    {
        private static int RoundToNearest(int value, int roundTo)
        {
            if (roundTo == 0)
            {
                return 0;
            }
            else if (value == 0)
            {
                return roundTo;
            }
            else
            {
                if (value < 0)
                    roundTo *= -1;

                int remainder = value % roundTo;
                return remainder != 0 ? value + roundTo - remainder : value;
            }
        }

        public readonly byte BitsPerBlock;
        public readonly byte BitsPerMetadata;
        public readonly ulong MaxBlockValue;
        public readonly ulong[] Data;
        public readonly int Size;

        public bool IsEmpty => Data.All(d => d.Equals(0UL));

        public BlockStorage64(byte bitsPerBlock, int size, byte bitsPerMetadata = 4)
            : this(bitsPerBlock, new ulong[RoundToNearest(size * bitsPerBlock, 64) / 64], bitsPerMetadata) { }

        public BlockStorage64(byte bitsPerBlock, in ulong[] data, byte bitsPerMetadata = 4)
        {
            BitsPerBlock = bitsPerBlock;
            BitsPerMetadata = bitsPerMetadata;
            Data = data;
            Size = data.Length * 64 / bitsPerBlock;
            MaxBlockValue = (1UL << BitsPerBlock) - 1UL;
        }

        public (ulong ID, ulong Metadata) this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Get(index);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Set(index, value.ID, value.Metadata);
        }

        public (ulong ID, ulong Metadata) Get(int index)
        {
            if (index < 0 || index > Size - 1)
                throw new IndexOutOfRangeException();

            ulong idMetadata;

            int bitIndex = index * BitsPerBlock;
            int startIndex = bitIndex / 64;
            int endIndex = (((index + 1) * BitsPerBlock) - 1) / 64;
            int startBitSubIndex = bitIndex % 64;
            if (startIndex == endIndex) // Data stored within one ulong
            {
                idMetadata = Data[startIndex] >> startBitSubIndex & MaxBlockValue;
            }
            else // Data stored within two ulongs
            {
                int endBitSubIndex = 64 - startBitSubIndex;
                idMetadata = Data[startIndex] >> startBitSubIndex | (Data[endIndex] << endBitSubIndex & MaxBlockValue);
            }

            return (idMetadata >> BitsPerMetadata, idMetadata & (MaxBlockValue >> (BitsPerBlock - BitsPerMetadata)));
        }

        public void Set(int index, ulong id, ulong metadata)
        {
            if (index < 0 || index > Size - 1)
                throw new IndexOutOfRangeException();

            var idMetadata = id << BitsPerMetadata | (metadata & (MaxBlockValue >> (BitsPerBlock - BitsPerMetadata)));

            if (idMetadata < 0 || idMetadata > MaxBlockValue)
                throw new ArgumentOutOfRangeException($"{nameof(id)} is greather than 2^{BitsPerBlock - BitsPerMetadata}");

            int bitIndex = index * BitsPerBlock;
            int startIndex = bitIndex / 64;
            int endIndex = (((index + 1) * BitsPerBlock) - 1) / 64;
            int startBitSubIndex = bitIndex % 64;
            Data[startIndex] = (Data[startIndex] & ~(MaxBlockValue << startBitSubIndex)) | (idMetadata & MaxBlockValue) << startBitSubIndex;
            if (startIndex != endIndex)
            {
                int endBitSubIndex = 64 - startBitSubIndex;
                Data[endIndex] = Data[endIndex] >> endBitSubIndex << endBitSubIndex | (idMetadata & MaxBlockValue) >> endBitSubIndex;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<(ulong ID, ulong Metadata)> GetEnumerator()
        {
            for(int i = 0; i < Size; i++)
                yield return Get(i);
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}