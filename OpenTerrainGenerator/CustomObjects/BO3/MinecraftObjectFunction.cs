using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{
    /**
     * Represents a block in a BO3.
     */
    public class MinecraftObjectFunction : BO3PlaceableFunction
{

    public DefaultStructurePart structurePart;
    public Rotation rotation = Rotation.NORTH;

    public MinecraftObjectFunction(BO3Config config, List<String> args) throws InvalidConfigException
    {
        super(config);
        assureSize(4, args);
        // Those limits are arbitrary, LocalWorld.setBlock will limit it
        // correctly based on what chunks can be accessed
        x = readInt(args.get(0), -100, 100);
        y = readInt(args.get(1), -1000, 1000);
        z = readInt(args.get(2), -100, 100);
        structurePart = DefaultStructurePart.getDefaultStructurePart(args.get(3));
    }

    public MinecraftObjectFunction(BO3Config holder)
    {
        super(holder);
    }

    @Override
    public String toString()
    {
        return "MinecraftObject(" + x + ',' + y + ',' + z + ',' + structurePart + ')';
    }

    @Override
    public MinecraftObjectFunction rotate()
    {
        MinecraftObjectFunction rotatedBlock = new MinecraftObjectFunction(getHolder());
        rotatedBlock.x = z;
        rotatedBlock.y = y;
        rotatedBlock.z = -x;
        rotatedBlock.rotation = rotation.next();

        return rotatedBlock;
    }

    @Override
    public void spawn(LocalWorld world, Random random, int x, int y, int z)
    {
        SpawnableObject object = world.getMojangStructurePart(structurePart.getPath());
        object.spawnForced(world, random, rotation, x, y, z);
    }

    @Override
    public boolean isAnalogousTo(ConfigFunction<BO3Config> other)
    {
        if(!getClass().equals(other.getClass())) {
            return false;
        }
        MinecraftObjectFunction block = (MinecraftObjectFunction) other;
        return block.x == x && block.y == y && block.z == z;
    }

}
