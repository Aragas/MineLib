using MineLib.Core.Anvil;

using System;

namespace MineLib.Core
{
    public static class Helper
    {
        /// <summary>
        /// Converting ushort value from a Chunk to a boolean value. Which Sections are empty or not.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool[] ConvertFromUShort(ushort value)
        {
            var array = new bool[15];

            for (var i = 0; i < 15; i++)
                array[i] = (value & (1 << i)) > 0;

            return array;
        }

        public static ushort ConvertToUShort(this Section[] sections)
        {
            ushort primaryBitMap = 0, mask = 1;
            foreach(var section in sections)
            {
                if (!section.IsEmpty)
                    primaryBitMap |= mask;

                mask <<= 1;
            }
            return primaryBitMap;
        }

        public static bool[] ConvertFromUShort(this Section[] sections)
        {
            if (sections.Length > 16)
                throw new NotSupportedException();

            var array = new bool[sections.Length];

            for (var i = 0; i < 15; i++)
            {
                if (!sections[i].IsEmpty)
                {
                    array[i] = true;
                }
            }

            return array;
        }
    }
}