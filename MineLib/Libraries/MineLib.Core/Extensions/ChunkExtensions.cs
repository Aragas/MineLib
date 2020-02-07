namespace MineLib.Core.Anvil
{
    public static class ChunkExtensions
    {
        public static ushort GetPrimaryBitMap(this in Chunk chunk) => chunk.Sections.ConvertToUShort();
        // -- Debugging
        public static bool[] GetPrimaryBitMapConverted(this in Chunk chunk) => chunk.Sections.ConvertFromUShort();
        // -- Debugging
    }
}