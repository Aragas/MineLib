using OpenTerrainGenerator.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{

    /**
     * UseWorld is a keyword that spawns the objects in the WorldObjects folder.
     */
    public class UseWorld : SimpleObject
    {
        public override void onEnable(Dictionary<String, CustomObject> otherObjectsInDirectory)
        {
            // Stub method
        }

        public override String getName() => "UseWorld";

        public override bool canSpawnAsTree() => true;

        public override bool canSpawnAsObject() => true;

        public override bool spawnForced(LocalWorld world, Random random, Rotation rotation, int x, int y, int z)
        {
            foreach (CustomObject @object in world.getConfigs().getCustomObjects())
            {
                if (@object.hasPreferenceToSpawnIn(world.getBiome(x, z)))
                {
                    if (@object.spawnForced(world, random, rotation, x, y, z))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool process(LocalWorld world, Random rand, ChunkCoordinate chunkCoord)
        {
            // Pick one object, try to spawn that, if that fails, try with another
            // object, as long as the objectSpawnRatio cap isn't reached.

            WorldConfig worldConfig = world.getConfigs().getWorldConfig();
            CustomObjectCollection customObjects = world.getConfigs().getCustomObjects();

            if (customObjects.isEmpty())
                return false;

            bool objectSpawned = false;
            int spawnattemps = 0;
            while (!objectSpawned)
            {
                if (spawnattemps > worldConfig.objectSpawnRatio)
                    return false;

                spawnattemps++;

                CustomObject selectedObject = customObjects.getRandomObject(rand);

                if (!selectedObject.hasPreferenceToSpawnIn(world.getBiome(chunkCoord.getBlockXCenter(), chunkCoord.getBlockZCenter())))
                    continue;

                // Process the object
                objectSpawned = selectedObject.process(world, rand, chunkCoord);

            }
            return objectSpawned;
        }

        public override CustomObject applySettings(SettingsMap settings)
        {
            // Not supported
            return this;
        }

        public override bool hasPreferenceToSpawnIn(LocalBiome biome)
        {
            // Never, ever spawn this in UseWorld. It would cause an infinite loop.
            return false;
        }

        public override bool canSpawnAt(LocalWorld world, Rotation rotation, int x, int y, int z)
        {
            return true; // We can only guess
        }

        public override bool canRotateRandomly()
        {
            return true;
        }
    }
}