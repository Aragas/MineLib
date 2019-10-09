using OpenTerrainGenerator.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{

    /**
     * Represents an object along with its location in the world.
     */
    public class CustomObjectCoordinate
    {

        private CustomObject @object;
        private Rotation rotation;
        private int x;
        private int y;
        private int z;

        public CustomObjectCoordinate(CustomObject @object, Rotation rotation, int x, int y, int z)
        {
            this.@object = @object;
            this.rotation = rotation;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getZ()
        {
            return z;
        }

        /**
         * Returns the object of this coordinate.
         *
         * @return The object.
         */
        public CustomObject getObject()
        {
            return @object;
        }

        public Rotation getRotation()
        {
            return rotation;
        }

        public bool spawnWithChecks(LocalWorld world, StructurePartSpawnHeight height, Random random)
        {
            y = height.getCorrectY(world, x, this.y, z);
            if (!@object.canSpawnAt(world, rotation, x, y, z))
            {
                return false;
            }
            return @object.spawnForced(world, random, rotation, x, y, z);
        }

        public override bool Equals(Object otherObject)
        {
            if (otherObject == null)
            {
                return false;
            }
            //if (!(otherObject instanceof CustomObjectCoordinate))
            //{
            //    return false;
            //}
            CustomObjectCoordinate otherCoord = (CustomObjectCoordinate)otherObject;
            if (otherCoord.x != x)
            {
                return false;
            }
            if (otherCoord.y != y)
            {
                return false;
            }
            if (otherCoord.z != z)
            {
                return false;
            }
            if (!otherCoord.rotation.Equals(rotation))
            {
                return false;
            }
            if (!otherCoord.@object.getName().Equals(@object.getName()))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return (x >> 13) ^ (y >> 7) ^ z ^ @object.getName().GetHashCode() ^ rotation.ToString().GetHashCode();
        }

        /**
         * Gets the chunk that should populate for this object.
         * @return The chunk.
         */
        public ChunkCoordinate getPopulatingChunk()
        {
            // In the past we simply returned the chunk populating for the origin
            // of the object. However, the origin is not guaranteed to be at the
            // center of the object. We need to know the exact center to choose
            // the appropriate spawning chunk.

            BoundingBox box = @object.getBoundingBox(rotation);
            int centerX = x + box.getMinX() + (box.getWidth() / 2);
            int centerZ = z + box.getMinZ() + (box.getDepth() / 2);

            return ChunkCoordinate.getPopulatingChunk(centerX, centerZ);
        }
    }
}