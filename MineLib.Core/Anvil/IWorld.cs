namespace MineLib.Core.Anvil
{
    public interface IWorld
    {
        ReadonlyBlock32 GetBlock(in Location3D blockWorldLocation);
        void SetBlock(in Location3D blockWorldLocation, in ReadonlyBlock32 block);
    }
}
