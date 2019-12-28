using OpenTerrainGenerator.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{

    /**
     * Each world has a cache of unfinished structures. This class is the cache.
     *
     */
    public class CustomObjectStructureCache
    {
        private Dictionary<ChunkCoordinate, CustomObjectStructure> structureCache;
        private LocalWorld world;

        public CustomObjectStructureCache(LocalWorld world)
        {
            this.world = world;
            this.structureCache = new Dictionary<ChunkCoordinate, CustomObjectStructure>();
        }

        public void reload(LocalWorld world)
        {
            this.world = world;
            structureCache.Clear();
        }

        public CustomObjectStructure getStructureStart(int chunkX, int chunkZ)
        {
            ChunkCoordinate coord = ChunkCoordinate.fromChunkCoords(chunkX, chunkZ);
            CustomObjectStructure structureStart = structureCache[coord];

            // Clear cache if needed
            if (structureCache.Count > 400)
            {
                structureCache.Clear();
            }

            if (structureStart != null)
            {
                return structureStart;
            }
            // No structure found, create one
            Random random = RandomHelper.getRandomForCoords(chunkX ^ 2, (chunkZ + 1) * 2, world.getSeed());
            CustomStructureGen structureGen = world.getBiome(chunkX * 16 + 15, chunkZ * 16 + 15).getBiomeConfig().structureGen;
            if (structureGen != null)
            {
                CustomObjectCoordinate customObject = structureGen.getRandomObjectCoordinate(random, chunkX, chunkZ);
                if (customObject != null)
                {
                    structureStart = new CustomObjectStructure(world, customObject);
                    structureCache[coord] = structureStart;
                    return structureStart;
                } // TODO Maybe also store that no structure was here?
            }

            return null;
        }
    }
}