namespace MineLib.Core.Anvil
{
    // 14 bit - 56kb
    // Total - 56kb
    public readonly struct BlockList
    {
        public readonly int XSize;
        public readonly int YSize;
        public readonly int ZSize;

        private readonly BlockStorage64 _blocks;

        public bool IsEmpty => _blocks.IsEmpty;

        public BlockList(int xSize, int ySize, int zSize)
            : this(xSize, ySize, zSize, new BlockStorage64(14, xSize * ySize * zSize)) { }

        public BlockList(int xSize, int ySize, int zSize, in BlockStorage64 blocks)
        {
            XSize = xSize;
            YSize = ySize;
            ZSize = zSize;

            _blocks = blocks;
        }

        //private int Index(int x, int y, int z) => x + (z * ZSize) + (y * YSize * XSize);
        //private static int Index(int x, int y, int z) => y << 8 | z << 4 | x;
        private int Index(int x, int y, int z) => x + (((y * YSize) + z) * XSize);

        public (ulong ID, ulong Metadata, byte Light, byte SkyLight) Get(int index)
        {
            var (ID, Meta) = _blocks[index];
            return (ID, Meta, 0, 0);
        }

        public (ulong ID, ulong Metadata, byte Light, byte SkyLight) Get(int x, int y, int z) => Get(Index(x, y, z));
        public (ulong ID, ulong Metadata, byte Light, byte SkyLight) Get(in Location3D pos) => Get(pos.X, pos.Y, pos.Z);

        public void Set(int index, (ulong ID, ulong Metadata, byte Light, byte SkyLight) value) => _blocks.Set(index, value.ID, value.Metadata);
        public void Set(int x, int y, int z, (ulong ID, ulong Metadata, byte Light, byte SkyLight) value) => Set(Index(x, y, z), value);
        public void Set(in Location3D pos, (ulong ID, ulong Metadata, byte Light, byte SkyLight) value) => Set(pos.X, pos.Y, pos.Z, value);

        public ReadonlyBlock32 GetBlock(int index)
        {
            var (ID, Meta) = _blocks[index];
            return new ReadonlyBlock32(ID, Meta);
        }

        public ReadonlyBlock32 GetBlock(int x, int y, int z) => GetBlock(Index(x, y, z));
        public ReadonlyBlock32 GetBlock(in Location3D pos) => GetBlock(pos.X, pos.Y, pos.Z);

        public void SetBlock(int index, in ReadonlyBlock32 value) => _blocks.Set(index, value.ID, value.Metadata);
        public void SetBlock(int x, int y, int z, in ReadonlyBlock32 value) => SetBlock(Index(x, y, z), value);
        public void SetBlock(in Location3D pos, in ReadonlyBlock32 value) => SetBlock(pos.X, pos.Y, pos.Z, in value);
    }
}