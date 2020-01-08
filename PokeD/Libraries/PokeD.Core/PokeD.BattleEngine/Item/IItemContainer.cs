namespace PokeD.BattleEngine.Item
{
    public interface IItemContainer
    {
        bool AttachItem(BaseItemInstance item);
        bool RemoveItem(BaseItemInstance item);

        bool HoldsItem(IItemStaticData item);
    }
}