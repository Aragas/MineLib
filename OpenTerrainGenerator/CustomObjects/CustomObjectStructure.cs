using OpenTerrainGenerator.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{

    /**
     * Represents a collection of all {@link CustomObject}s in a structure. It is
     * calculated by finding the branches of one object, then finding the branches
     * of those branches, etc., until
     * {@link CustomObject#getMaxBranchDepth()} is reached.
     *
     */
    public class CustomObjectStructure
    {
        protected Random random;
    protected LocalWorld world;
        protected CustomObjectCoordinate start;
        protected StructurePartSpawnHeight height;
        private Dictionary<ChunkCoordinate, List<CustomObjectCoordinate>> objectsToSpawn;
        private int maxBranchDepth;

        public CustomObjectStructure(LocalWorld world, CustomObjectCoordinate start)
        {
            CustomObject @object = start.getObject();

            this.world = world;
            this.start = start;
            this.height = @object.getStructurePartSpawnHeight();
            this.maxBranchDepth = @object.getMaxBranchDepth();
            random = RandomHelper.getRandomForCoords(start.getX(), start.getY(), start.getZ(), world.getSeed());

            // Calculate all branches and add them to a list
            objectsToSpawn = new Dictionary<ChunkCoordinate, List<CustomObjectCoordinate>>();
            addToSpawnList(start); // Add the object itself
            addBranches(start, 1);
        }

        private void addBranches(CustomObjectCoordinate coordObject, int depth)
        {
            foreach (Branch branch in getBranches(coordObject.getObject(), coordObject.getRotation()))
            {
                CustomObjectCoordinate childCoordObject = branch.toCustomObjectCoordinate(world, random, coordObject.getX(),
                        coordObject.getY(), coordObject.getZ());

                // Don't add null objects
                if (childCoordObject == null)
                {
                    continue;
                }

                // Add this object to the chunk
                addToSpawnList(childCoordObject);

                // Also add the branches of this object
                if (depth < maxBranchDepth)
                {
                    addBranches(childCoordObject, depth + 1);
                }
            }
        }

        private Branch[] getBranches(CustomObject customObject, Rotation rotation)
        {
            return customObject.getBranches(rotation);
        }

        /**
         * Adds the object to the spawn list of each chunk that the object
         * touches.
         * @param coordObject The object.
         */
        private void addToSpawnList(CustomObjectCoordinate coordObject)
        {
            ChunkCoordinate chunkCoordinate = coordObject.getPopulatingChunk();

            List<CustomObjectCoordinate> objectsInChunk = objectsToSpawn[chunkCoordinate];
            if (objectsInChunk == null)
            {
                objectsInChunk = new List<CustomObjectCoordinate>();
                objectsToSpawn[chunkCoordinate] = objectsInChunk;
            }
            objectsInChunk.Add(coordObject);
        }

        /**
         * Spawns all the objects that should be spawned in that chunk.
         * @param chunkCoordinate The chunk to spawn in.
         */
        public void spawnForChunk(ChunkCoordinate chunkCoordinate)
        {
            List<CustomObjectCoordinate> objectsInChunk = objectsToSpawn[chunkCoordinate];
            if (objectsInChunk != null)
            {
                foreach (CustomObjectCoordinate coordObject in objectsInChunk)
                {
                    coordObject.spawnWithChecks(world, height, random);
                }
            }
        }
    }
}