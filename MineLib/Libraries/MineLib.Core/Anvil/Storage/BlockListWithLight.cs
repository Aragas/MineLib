namespace MineLib.Core.Anvil
{
    // 14 bit - 56kb
    // 2kb
    // 2kb
    // Total - 60kb
    public readonly struct BlockListWithLight
    {
        public readonly int XSize;
        public readonly int YSize;
        public readonly int ZSize;

        public readonly BlockStorage64 Blocks;
        public readonly NibbleArray BlockLight;
        public readonly NibbleArray BlockSkyLight;

        public bool IsEmpty => Blocks.IsEmpty;

        public BlockListWithLight(int xSize, int ySize, int zSize)
            : this(xSize, ySize, zSize, new BlockStorage64(14, xSize * ySize * zSize), new NibbleArray(xSize * ySize * zSize), new NibbleArray(xSize * ySize * zSize)) { }

        public BlockListWithLight(int xSize, int ySize, int zSize, in BlockStorage64 blocks, in NibbleArray blockLight, in NibbleArray blockSkyLight)
        {
            XSize = xSize;
            YSize = ySize;
            ZSize = zSize;

            Blocks = blocks;
            BlockLight = blockLight;
            BlockSkyLight = blockSkyLight;
        }

        //private int Index(int x, int y, int z) => x + (z * ZSize) + (y * YSize * XSize);
        private static int Index(int x, int y, int z) => y << 8 | z << 4 | x;
        //private static int Index(int x, int y, int z) => x + ((y * YSize) + z) * XSize;

        public (ulong ID, ulong Metadata, byte Light, byte SkyLight) Get(int index)
        {
            var (ID, Meta) = Blocks[index];
            return (ID, Meta, BlockLight[index], BlockSkyLight[index]);
        }

        public (ulong ID, ulong Metadata, byte Light, byte SkyLight) Get(int x, int y, int z) => Get(Index(x, y, z)); //=> Get(x + ((y * YSize) + z) * XSize);
        public (ulong ID, ulong Metadata, byte Light, byte SkyLight) Get(in Location3D pos) => Get(pos.X, pos.Y, pos.Z);

        public void Set(int index, (ulong ID, ulong Metadata, byte Light, byte SkyLight) value)
        {
            Blocks.Set(index, value.ID, value.Metadata);
            BlockLight.Set(index, value.Light);
            BlockSkyLight.Set(index, value.SkyLight);
        }

        public void Set(int x, int y, int z, (ulong ID, ulong Metadata, byte Light, byte SkyLight) value) => Set(Index(x, y, z), value); //=> Set(x + ((y * YSize) + z) * XSize, value);
        public void Set(in Location3D pos, (ulong ID, ulong Metadata, byte Light, byte SkyLight) value) => Set(pos.X, pos.Y, pos.Z, value);

        public ReadonlyBlock32 GetBlock(int index)
        {
            var (ID, Meta) = Blocks[index];
            return new ReadonlyBlock32(ID, Meta, BlockLight[index], BlockSkyLight[index]);
        }

        public ReadonlyBlock32 GetBlock(int x, int y, int z) => GetBlock(Index(x, y, z));
        public ReadonlyBlock32 GetBlock(in Location3D pos) => GetBlock(pos.X, pos.Y, pos.Z);

        public void SetBlock(int index, in ReadonlyBlock32 value)
        {
            Blocks.Set(index, value.ID, value.Metadata);
            BlockLight.Set(index, value.Light);
            BlockSkyLight.Set(index, value.SkyLight);
        }

        public void SetBlock(int x, int y, int z, in ReadonlyBlock32 value) => SetBlock(Index(x, y, z), in value); //=> SetBlock(x + ((y * YSize) + z) * XSize, value);
        public void SetBlock(in Location3D pos, in ReadonlyBlock32 value) => SetBlock(pos.X, pos.Y, pos.Z, in value);
    }
}