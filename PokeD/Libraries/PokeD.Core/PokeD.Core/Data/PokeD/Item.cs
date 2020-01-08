using PokeD.BattleEngine.Item;

namespace PokeD.Core.Data.PokeD
{
    public class Item : BaseItemInstance
    {
        public Item(int id) : base(Cached<ItemStaticData>.Get(id)) { }
    }
}
