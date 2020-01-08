namespace PokeD.Core.Extensions
{
    public static class BitMaskExtensions
    {
        public static long BitsGet(this long number, int start, int end)
        {
            var mask = ~(~0 << (end - start));
            return (number >> start) & mask;
        }
        public static long BitsSet(this long number, long value, int start, int end)
        {
            var mask = BitsGet(value, 0, end - start) << start;
            return number | mask;
        }

        public static int BitsGet(this int number, int start, int end)
        {
            var mask = ~(~0 << (end - start));
            return (number >> start) & mask;
        }
        public static int BitsSet(this int number, int value, int start, int end)
        {
            var mask = BitsGet(value, 0, end - start) << start;
            return number | mask;
        }

        public static short BitsGet(this short number, int start, int end)
        {
            var mask = ~(~0 << (end - start));
            return (short) ((number >> start) & mask);
        }
        public static short BitsSet(this short number, short value, int start, int end)
        {
            var mask = BitsGet(value, 0, end - start) << start;
            return (short) (number | mask);
        }

        public static byte BitsGet(this byte number, int start, int end)
        {
            var mask = ~(~0 << (end - start));
            return (byte) ((number >> start) & mask);
        }
        public static byte BitsSet(this byte number, byte value, int start, int end)
        {
            var mask = BitsGet(value, 0, end - start) << start;
            return (byte) (number | mask);
        }
    }
}