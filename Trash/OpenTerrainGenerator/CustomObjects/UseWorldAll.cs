using OpenTerrainGenerator.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{
    public class UseWorldAll : UseWorld
    {
        public override String getName()
        {
            return "UseWorldAll";
        }

        public override bool process(LocalWorld world, Random rand, ChunkCoordinate chunkCoord)
        {
            bool spawnedAtLeastOneObject = false;

            foreach (var selectedObject in world.getConfigs().getCustomObjects())
            {
                if (!selectedObject.hasPreferenceToSpawnIn(world.getBiome(chunkCoord.getBlockXCenter(), chunkCoord.getBlockZCenter())))
                    continue;

                // Process the object
                if (selectedObject.process(world, rand, chunkCoord))
                {
                    spawnedAtLeastOneObject = true;
                }
            }
            return spawnedAtLeastOneObject;
        }
    }
}