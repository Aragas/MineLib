using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{

    /**
     * A Minecraft tree, viewed as a custom object.
     *
     * <p>For historical reasons, TreeObject implements {@link CustomObject} instead
     * of just {@link SpawnableObject}. We can probably refactor the Tree resource
     * to accept {@link SpawnableObject}s instead of {@link CustomObject}s, so that
     * all the extra methods are no longer needed.
     */
    public class TreeObject : SimpleObject
    {
        private static class TreeSettings : Settings
        {
            static Setting<Integer> MIN_HEIGHT = intSetting("MinHeight",
                    TerrainControl.WORLD_DEPTH, TerrainControl.WORLD_DEPTH, TerrainControl.WORLD_HEIGHT);
            static Setting<Integer> MAX_HEIGHT = intSetting("MaxHeight",
                    TerrainControl.WORLD_HEIGHT, TerrainControl.WORLD_DEPTH, TerrainControl.WORLD_HEIGHT);
        }

        private TreeType type;
        private int minHeight = TerrainControl.WORLD_DEPTH;
        private int maxHeight = TerrainControl.WORLD_HEIGHT;

        public TreeObject(TreeType type)
        {
            this.type = type;
        }

        public override void onEnable(Map<String, CustomObject> otherObjectsInDirectory)
        {
            // Stub method
        }

        public TreeObject(TreeType type, SettingsMap settings)
        {
            this.type = type;
            this.minHeight = settings.getSetting(TreeSettings.MIN_HEIGHT, TreeSettings.MIN_HEIGHT.getDefaultValue());
            this.maxHeight = settings.getSetting(TreeSettings.MAX_HEIGHT, TreeSettings.MAX_HEIGHT.getDefaultValue());
        }

        public override String getName()
        {
            return type.name();
        }

        public override bool canSpawnAsTree()
        {
            return true;
        }

        public override bool canSpawnAsObject()
        {
            return false;
        }

        public override bool spawnForced(LocalWorld world, Random random, Rotation rotation, int x, int y, int z)
        {
            return world.placeTree(type, random, x, y, z);
        }

        public override bool process(LocalWorld world, Random random, ChunkCoordinate chunkCoord)
        {
            // A tree has no frequency or rarity, so spawn it once in the chunk
            int x = chunkCoord.getBlockXCenter() + random.nextInt(ChunkCoordinate.CHUNK_X_SIZE);
            int z = chunkCoord.getBlockZCenter() + random.nextInt(ChunkCoordinate.CHUNK_Z_SIZE);
            int y = world.getHighestBlockYAt(x, z);
            if (canSpawnAt(world, Rotation.NORTH, x, y, z))
            {
                return spawnForced(world, random, Rotation.NORTH, x, y, z);
            }
            return false;
        }

        public override CustomObject applySettings(SettingsMap settings)
        {
            return new TreeObject(type, settings);
        }

        public override bool hasPreferenceToSpawnIn(LocalBiome biome)
        {
            return true;
        }

        public override bool canSpawnAt(LocalWorld world, Rotation rotation, int x, int y, int z)
        {
            if (y < minHeight || y > maxHeight)
            {
                return false;
            }
            return true;
        }

    }
}