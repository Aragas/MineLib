using PokeD.Core.Extensions;

namespace PokeD.Core.Data.PokeD.Structs
{
    public struct MetaItem
    {
        public short Meta { get; private set; }

        /// <summary>
        /// Index of used Item.
        /// </summary>
        /// <remarks>Range 0-1023, used 0-~800.</remarks>
        public short Item { get => Meta.BitsGet(0, 10); set => Meta = Meta.BitsSet(value, 0, 10); }

        /// <summary>
        /// Index of used Monster.
        /// </summary>
        /// <remarks>Range 0-3, used 0-1.</remarks>
        public short Monster { get => Meta.BitsGet(14, 16); set => Meta = Meta.BitsSet(value, 14, 16); }


        public MetaItem(short item, short monster) : this() { Item = item; Monster = monster; }
        public MetaItem(short meta) { Meta = meta; }
    }
}