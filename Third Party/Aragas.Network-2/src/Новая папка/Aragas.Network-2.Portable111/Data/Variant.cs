using System;
using System.Buffers;
using System.IO;

namespace Aragas.Network.Data
{
    internal static class Variant
    {
        internal static int VariantSize(ulong value)
        {
            var outputSize = 0;
            while (value > 127)
            {
                outputSize++;
                value >>= 7;
            }
            return ++outputSize;
        }
        internal static int VariantSize(long value)
        {
            var outputSize = 0;
            while (value > 127)
            {
                outputSize++;
                value >>= 7;
            }
            return ++outputSize;
        }

        internal static byte[] Encode(ulong value)
        {
            var size = VariantSize(value);
            var array = new byte[size];

            for (var i = 0; i < array.Length - 1; i++)
            {
                array[i] = (byte) ((value & 127) | 128u);
                value >>= 7;
            }
            array[array.Length - 1] = (byte) value;

            return array;
        }

        internal static ulong? Decode(in ReadOnlySequence<byte> sequence)
        {
            if(sequence.IsSingleSegment)
                return Decode(sequence.First.Span);

            int sequenceIndex = 0;
            ulong decodedValue = 0;
            foreach (var memory in sequence)
            {
                sequenceIndex += memory.Length;

                var span = memory.Span;
                int index = 0, shiftAmount = 0;
                byte currByte = 0;
                do
                {
                    if (index >= span.Length)
                    {
                        if(sequence.End.Equals(sequence.GetPosition(sequenceIndex)))
                        {
                            return null; // End of sequence, data not sufficient
                        }

                        continue; // Continue reading from next sequence
                    }

                    currByte = span[index++];
                    ulong lowByte = currByte & 127u;
                    decodedValue |= lowByte << shiftAmount++ * 7;
                } while ((currByte & 128u) != 0);
            }
            return decodedValue;
        }
        internal static ulong Decode(in ReadOnlySpan<byte> buffer)
        {
            ulong decodedValue = 0;
            int index = 0, shiftAmount = 0;
            byte currByte = 0;
            do
            {
                currByte = buffer[index++];
                ulong lowByte = currByte & 127u;
                decodedValue |= lowByte << shiftAmount++ * 7;
            } while ((currByte & 128u) != 0);

            return decodedValue;
        }
        internal static ulong Decode(byte[] buffer, int offset)
        {
            ulong decodedValue = 0;
            int index = 0, shiftAmount = 0;
            byte currByte = 0;
            do
            {
                currByte = buffer[offset + index++];
                ulong lowByte = currByte & 127u;
                decodedValue |= lowByte << shiftAmount++ * 7;
            } while ((currByte & 128u) != 0);

            return decodedValue;
        }
        internal static ulong Decode(Stream stream)
        {
            ulong decodedValue = 0;
            int shiftAmount = 0;
            byte currByte = 0;
            do
            {
                currByte = (byte) stream.ReadByte();
                ulong lowByte = currByte & 127u;
                decodedValue |= lowByte << shiftAmount++ * 7;
            } while ((currByte & 128u) != 0);

            return decodedValue;
        }

        internal static long ZigZagEncode(long value) => (value << 1) ^ (value >> 63);
        internal static long ZigZagDecode(long value)
        {
            var temp = (((value << 63) >> 63) ^ value) >> 1;
            return temp ^ (value & (1L << 63));
        }
    }
}