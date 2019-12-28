using MineLib.Core;
using MineLib.Core.Anvil;

namespace MineLib.Server.WorldBus
{
    public interface IWorldHandler
    {
        Chunk GetChunk(Location2D location);
        void SetChunk(in Chunk chunk);

        Section GetSection(Location3D chunkLocation);
        void SetSection(in Section section);

        ReadonlyBlock32 GetBlock(in Location3D blockWorldLocation);
        void SetBlock(in Location3D blockWorldLocation, in ReadonlyBlock32 block);
    }
}