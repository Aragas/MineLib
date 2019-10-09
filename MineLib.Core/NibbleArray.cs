using System.Linq;

namespace MineLib.Core.Anvil
{
    public readonly struct NibbleArray
    {
        /// <summary>
        /// The data in the nibble array. Each byte contains
        /// two nibbles, stored in big-endian.
        /// </summary>
        public readonly byte[] Data;

        public bool IsEmpty => Data.All(d => d.Equals(byte.MinValue));

        public NibbleArray(in byte[] data) => Data = data;

        /// <summary>
        /// Creates a new nibble array with the given number of nibbles.
        /// </summary>
        public NibbleArray(int length) => Data = new byte[((length - 1) / 2) + 1];

        /// <summary>
        /// Gets the current number of nibbles in this array.
        /// </summary>
        public int Length => Data.Length * 2;

        /// <summary>
        /// Gets or sets a nibble at the given index.
        /// </summary>
        public byte this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public byte Get(int index) => (byte) (Data[index / 2] >> (index % 2 * 4) & 0b00001111);

        public void Set(int index, byte value)
        {
            value &= 0b00001111;
            Data[index / 2] &= (byte) (0b00001111 << ((index + 1) % 2 * 4));
            Data[index / 2] |= (byte) (value << (index % 2 * 4));
        }
    }
}