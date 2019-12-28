using OpenTerrainGenerator.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{
    /**
     * Base class for simple custom objects: custom objects that have no branches,
     * no interesting bounding box and that cannot rotate.
     */
    public abstract class SimpleObject : CustomObject
    {
        public override bool canRotateRandomly() => false;

        public override BoundingBox getBoundingBox(Rotation rotation) => BoundingBox.newEmptyBox();

        public override Branch[] getBranches(Rotation rotation) => new Branch[0];

        public override int getMaxBranchDepth() => 0;

        public override StructurePartSpawnHeight getStructurePartSpawnHeight() => StructurePartSpawnHeight.PROVIDED;

        public override bool hasBranches() => false;

        // Cannot start a structure using this object
        public override CustomObjectCoordinate makeCustomObjectCoordinate(Random random, int chunkX, int chunkZ) => null;
    }
}