using System.Collections.Generic;

namespace PokeD.BattleEngine.Item
{
    public class Bag : IItemContainer
    {
        public List<BaseItemInstance> Items { get; } = new List<BaseItemInstance>();

        public bool AttachItem(BaseItemInstance item)
        {
            Items.Add(item);
            return true;
        }
        public bool RemoveItem(BaseItemInstance item)
        {
            Items.Remove(item);
            return true;
        }


        public bool HoldsItem(IItemStaticData item) => Items.Exists(i => i.StaticData == item);
    }
}