using PokeD.Core.Extensions;

namespace PokeD.Core.Data.PokeD.Structs
{
    public struct MetaAttack
    {
        public byte Meta { get; private set; }

        /// <summary>
        /// Index of used Monster.
        /// </summary>
        /// <remarks>Range 0-3, used 0-1.</remarks>
        public byte CurrentMonster { get => Meta.BitsGet(0, 2); set => Meta = Meta.BitsSet(value, 0, 2); }

        /// <summary>
        /// Index of used Move.
        /// </summary>
        /// <remarks>Range 0-3, used 0-3.</remarks>
        public byte Move { get => Meta.BitsGet(2, 4); set => Meta = Meta.BitsSet(value, 2, 4); }

        /// <summary>
        /// Index of used Monster. 16 is All. 15 is All except Attacker
        /// </summary>
        /// <remarks>Range 0-15, used 0-15.</remarks>
        public byte TargetMonster { get => Meta.BitsGet(4, 8); set => Meta = Meta.BitsSet(value, 4, 8); }


        public MetaAttack(byte currentMonster, byte move, byte targetMonster) : this() { CurrentMonster = currentMonster; Move = move; TargetMonster = targetMonster; }
        public MetaAttack(byte meta) { Meta = meta; }
    }
}