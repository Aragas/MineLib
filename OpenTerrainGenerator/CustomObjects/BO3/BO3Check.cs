using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{
    /**
     * Represents a check - something that can prevent the BO3 from spawning if this
     * condition is not met.
     */
    public abstract class BO3Check : BO3Function
{

    /** 
     * X position relative to the object origin.
     */
    public int x;
    /** 
     * Y position relative to the object origin.
     */
    public int y;
    /** 
     * Z position relative to the object origin.
     */
    public int z;

    public BO3Check(BO3Config holder)
    {
        super(holder);
    }

    /**
     * Returns whether this check would prevent spawning at the given position.
     * The given x, y and z positions are simply the relative coords in this
     * object added to the coords of the origin of the BO3. The internal
     * coords in this object should be ignored.
     *
     * @param world The world to check in
     * @param x     The x position
     * @param y     The y position
     * @param z     The z position
     * @return Whether this check prevents the BO3 from spawning.
     */
    public abstract bool preventsSpawn(LocalWorld world, int x, int y, int z);

    /**
     * This implementation of
     * {@link ConfigFunction#isAnalogousTo(ConfigFunction)} simply checks
     * whether the classes and coordinates are the same.
     */
    public override bool isAnalogousTo(ConfigFunction<BO3Config> other)
    {
        if(!GetType().equals(other.GetType())) {
            return false;
        }
        BO3Check check = (BO3Check) other;
        return check.x == x && check.y == y && check.z == z;
    }

    public abstract BO3Check rotate();
}
