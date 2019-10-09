using OpenTerrainGenerator.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{

    /**
     * UseBiome is a keyword that spawns the objects in the BiomeConfig/BiomeObjects
     * setting.
     */
    public class UseBiome : SimpleObject
    {
        public List<CustomObject> getPossibleObjectsAt(LocalWorld world, int x, int z)
        {
            return world.getBiome(x, z).getBiomeConfig().biomeObjects;
        }

        public override void onEnable(Dictionary<String, CustomObject> otherObjectsInDirectory)
        {
            // Stub method
        }

        public override String getName()
        {
            return "UseBiome";
        }

        public override bool canSpawnAsTree()
        {
            return true;
        }

        public override bool canSpawnAsObject()
        {
            return true;
        }

        public override bool spawnForced(LocalWorld world, Random random, Rotation rotation, int x, int y, int z)
        {
            foreach (CustomObject @object in getPossibleObjectsAt(world, x, z))
            {
                if (@object.spawnForced(world, random, rotation, x, y, z))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool process(LocalWorld world, Random random, ChunkCoordinate chunkCoord)
        {
            List<CustomObject> possibleObjects = getPossibleObjectsAt(world, chunkCoord.getBlockXCenter(), chunkCoord.getBlockZCenter());

            // Pick one object, try to spawn that, if that fails, try with another
            // object, as long as the objectSpawnRatio cap isn't reached.
            int objectSpawnRatio = world.getConfigs().getWorldConfig().objectSpawnRatio;

            if (possibleObjects.isEmpty())
                return false;

            boolean objectSpawned = false;
            int spawnattemps = 0;
            while (!objectSpawned)
            {
                if (spawnattemps > objectSpawnRatio)
                    return false;

                spawnattemps++;

                CustomObject selectedObject = possibleObjects.get(random.nextInt(possibleObjects.size()));

                // Process the object
                objectSpawned = selectedObject.process(world, random, chunkCoord);

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
            // Never, ever spawn this with UseWorld.
            return false;
        }

        public override bool canSpawnAt(LocalWorld world, Rotation rotation, int x, int y, int z)
        {
            List<CustomObject> objects = getPossibleObjectsAt(world, x, z);
            if (objects.isEmpty())
            {
                // No objects to spawn
                return false;
            }
            // Check for all the object
            foreach (CustomObject @object in objects)
            {
                if (!@object.canSpawnAt(world, rotation, x, y, z))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool canRotateRandomly()
        {
            return true;
        }
    }
}