using MineLib.Core.Anvil;

namespace MineLib.Protocol5.Extensions
{
    public static class BlockListWithLightExtensions
    {
        public static byte[] GetBlockIDs(this in BlockListWithLight storage)
        {
            var size = storage.XSize * storage.YSize * storage.ZSize;
            var array = new byte[size];
            for (int i = 0; i < size; i++)
                array[i] = (byte) storage.Get(i).ID;
            return array;
        }
        public static NibbleArray GetBlockMetadatas(this in BlockListWithLight storage)
        {
            var size = storage.XSize * storage.YSize * storage.ZSize;
            var array = new byte[size];
            for (int i = 0; i < size; i++)
                array[i] = (byte) storage.Get(i).Metadata;
            return new NibbleArray(in array);
        }
        public static NibbleArray GetBlockLight(this in BlockListWithLight storage) => storage.BlockLight;
        public static NibbleArray GetBlockSkyLight(this in BlockListWithLight storage) => storage.BlockSkyLight;
    }
}