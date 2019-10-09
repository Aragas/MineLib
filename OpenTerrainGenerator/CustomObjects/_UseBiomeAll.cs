using OpenTerrainGenerator.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{

    public class UseBiomeAll : UseBiome
    {
        public override String getName()
        {
            return "UseBiomeAll";
        }

        public override bool process(LocalWorld world, Random random, ChunkCoordinate chunkCoord)
        {
            bool spawnedAtLeastOneObject = false;

            foreach (CustomObject @object in getPossibleObjectsAt(world, chunkCoord.getBlockXCenter(), chunkCoord.getBlockZCenter()))
            {
                if (@object.process(world, random, chunkCoord))
                {
                    spawnedAtLeastOneObject = true;
                }
            }

            return spawnedAtLeastOneObject;
        }
    }
}