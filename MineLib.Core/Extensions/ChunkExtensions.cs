using MineLib.Core.Anvil;

namespace MineLib.Core.Extensions
{
    public static class ChunkExtensions
    {
        public static ushort GetPrimaryBitMap(this in Chunk chunk) => chunk.Sections.ConvertToUShort();
        // -- Debugging
        public static bool[] GetPrimaryBitMapConverted(this in Chunk chunk) => chunk.Sections.ConvertFromUShort();
        // -- Debugging
    }
}