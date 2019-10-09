using System.Runtime.InteropServices;

namespace MineLib.Core.Anvil
{
    // 96b
    // 60kb
    // Total - 61536b -> 61kb
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Section
    {
        /// <param name="sectionLocation">Relative to Chunk</param>
        public static Section BuildEmpty(in Location3D sectionLocation)
            => new Section(in sectionLocation, new BlockListWithLight(Width, Height, Depth));

        /// <param name="sectionLocation">Relative to Chunk</param>
        public static Section BuildFromData(in Location3D sectionLocation, in BlockStorage64 blocks, in NibbleArray blockLights, in NibbleArray blockSkyLights)
            => new Section(in sectionLocation, new BlockListWithLight(Width, Height, Depth, in blocks, in blockLights, in blockSkyLights));

        /// <param name="sectionLocation">Relative to Chunk</param>
        public static Section BuildFromBlocks(in Location3D sectionLocation, in BlockListWithLight blockList)
            => new Section(in sectionLocation, in blockList);

        /// <param name="sectionLocation">Relative to Chunk</param>
        public static Section BuildFromBlocks(in Location3D sectionLocation, in ReadonlyBlock32[] blocks)
        {
            var blockList = new BlockListWithLight(Width, Height, Depth);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = 0; z < Depth; z++)
                    {
                        blockList.SetBlock(x, y, z, in blocks[x + (((y * Height) + z) * Width)]);
                    }
                }
            }

            /*
            for (int i = 0, j = 0; i < Width * Height * Depth; i++)
            {
                //var idMetadata = BitConverter.ToUInt16(new[] { blocks[j], blocks[j + 1] }, 0);
                var idMetadata = (blocks[j + 1] << 8) | blocks[j];

                var id = (ushort) (idMetadata >> 4);
                var meta = (byte) (idMetadata & 0b0000000000001111); // & 15

                Blocks[i] = new Block(id, meta, blockLight[i], skyLight[i]);

                j = j + 2;
            }
            */

            return new Section(in sectionLocation, in blockList);
        }

        public const int Width  = 16;
        public const int Height = 16;
        public const int Depth  = 16;

        public readonly Location3D Location;
        public readonly BlockListWithLight Storage;
        public bool IsEmpty => Storage.IsEmpty;

        /// <param name="sectionLocation">Relative to Chunk</param>
        public Section(in Location3D sectionLocation, in BlockListWithLight storage)
        {
            Location = sectionLocation;
            Storage = storage;
        }

        /// <param name="location">Location relative to Section</param>
        public BlockLocation GetHighestBlockAtY(in Location3D location) => GetHighestBlockAtY(new Location2D(location.X, location.Z));
        /// <param name="location">Location relative to Section</param>
        public BlockLocation GetHighestBlockAtY(in Location2D location)
        {
            ReadonlyBlock32 block = default;
            Location3D blockLocation = default;

            for (var y = 0; y < Depth; y++)
            {
                blockLocation = new Location3D(location.X, y, location.Z);
                block = GetBlock(blockLocation);
                if (!block.Equals(ReadonlyBlock32.Empty))
                    break;
            }
    ;
            return new BlockLocation(in block, in blockLocation);
        }


        public ReadonlyBlock32 GetBlock(in Location3D blockSectionLocation) => Storage.GetBlock(blockSectionLocation.X, blockSectionLocation.Y, blockSectionLocation.Z);
        public void SetBlock(in Location3D blockSectionLocation, in ReadonlyBlock32 block) => Storage.SetBlock(blockSectionLocation.X, blockSectionLocation.Y, blockSectionLocation.Z, in block);

        public byte GetBlockLighting(in Location3D blockSectionLocation) => Storage.Get(blockSectionLocation.X, blockSectionLocation.Y, blockSectionLocation.Z).Light;
        public void SetBlockLighting(in Location3D blockSectionLocation, byte light)
        {
            var data = Storage.Get(blockSectionLocation.X, blockSectionLocation.Y, blockSectionLocation.Z);
            data.Light = light;
            Storage.Set(blockSectionLocation.X, blockSectionLocation.Y, blockSectionLocation.Z, data);
        }

        public byte GetBlockSkylight(in Location3D blockSectionLocation) => Storage.Get(blockSectionLocation.X, blockSectionLocation.Y, blockSectionLocation.Z).SkyLight;
        public void SetBlockSkylight(in Location3D blockSectionLocation, byte skyLight)
        {
            var data = Storage.Get(blockSectionLocation.X, blockSectionLocation.Y, blockSectionLocation.Z);
            data.SkyLight = skyLight;
            Storage.Set(blockSectionLocation.X, blockSectionLocation.Y, blockSectionLocation.Z, data);
        }

        public override string ToString() => IsEmpty ? "Empty" : "Filled";

        public override int GetHashCode() => Location.GetHashCode();
    }
}