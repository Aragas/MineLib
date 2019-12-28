using System;

namespace MineLib.Core.Anvil
{
    public readonly struct BlockLocation : IEquatable<BlockLocation>
    {
        public readonly ReadonlyBlock32 Block;
        public readonly Location3D WorldLocation;

        public BlockLocation(in ReadonlyBlock32 block, in Location3D worldLocation)
        {
            Block = block;
            WorldLocation = worldLocation;
        }

        public static bool operator ==(in BlockLocation a, in BlockLocation b) => a.Block == b.Block && a.WorldLocation == b.WorldLocation;
        public static bool operator !=(in BlockLocation a, in BlockLocation b) => !(a == b);

        public override bool Equals(object obj) => obj is BlockLocation blockLocation && Equals(blockLocation);
        public bool Equals(BlockLocation other) => Block.Equals(other.Block) && WorldLocation.Equals(other.WorldLocation);

        public override int GetHashCode() => HashCode.Combine(Block, WorldLocation);
    }
}