using MineLib.Core.Anvil;

namespace MineLib.Protocol5.Extensions
{
    public static class BlockStorage64Extensions
    {
        public static (byte[] IDs, NibbleArray Metadatas) Convert(this in BlockStorage64 storage)
        {
            var idArray = new byte[storage.Size];
            var metadataArray = new NibbleArray(storage.Size);
            for (int index = 0; index < storage.Size; index++)
            {
                ulong idMetadata;

                int bitIndex = index * storage.BitsPerBlock;
                int startIndex = bitIndex / 64;
                int endIndex = (((index + 1) * storage.BitsPerBlock) - 1) / 64;
                int startBitSubIndex = bitIndex % 64;
                if (startIndex == endIndex) // Data stored within one ulong
                {
                    idMetadata = storage.Data[startIndex] >> startBitSubIndex & storage.MaxBlockValue;
                }
                else // Data stored within two ulongs
                {
                    int endBitSubIndex = 64 - startBitSubIndex;
                    idMetadata = storage.Data[startIndex] >> startBitSubIndex | (storage.Data[endIndex] << endBitSubIndex & storage.MaxBlockValue);
                }

                idArray[index] = (byte) (idMetadata >> storage.BitsPerMetadata);
                metadataArray[index] = (byte) (idMetadata & (storage.MaxBlockValue >> (storage.BitsPerBlock - storage.BitsPerMetadata)));
            }
            return (idArray, metadataArray);
        }
    }
}