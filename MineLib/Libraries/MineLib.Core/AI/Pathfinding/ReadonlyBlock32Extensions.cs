using MineLib.Core.Anvil;

namespace MineLib.Core.AI.Pathfinding
{
    public static class ReadonlyBlock32Extensions
    {
        public static Material GetMaterial(this in ReadonlyBlock32 block) => (Material) block.ID;
    }
}