using MineLib.Core;
using MineLib.Core.Anvil;

using System.Numerics;

namespace MineLib.Server.WorldBus.Generator
{
    public interface IWorldGenerator
    {
        string LevelType { get; }
        string GeneratorName { get; }
        string GeneratorOptions { get; set; }
        long Seed { get; set; }
        Vector3 SpawnPoint { get; }

        Chunk GenerateChunk(Location2D location);

        /// <summary>
        /// Called after the world generator is created and
        /// all values are set.
        /// </summary>
        void Initialize(Level level);
    }
}