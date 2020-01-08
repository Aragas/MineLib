namespace PokeD.BattleEngine.Item.Data
{
    public interface IItemAttribute { }
    public class ItemAttributeCountable : IItemAttribute
    {

    }
    public class ItemAttributeConsumable : IItemAttribute
    {

    }
    public class ItemAttributeUsableOverworld : IItemAttribute
    {

    }
    public class ItemAttributeUsableInBattle : IItemAttribute
    {

    }
    public class ItemAttributeHoldable : IItemAttribute
    {

    }
    public class ItemAttributeHoldablePassive : ItemAttributeHoldable
    {

    }
    public class ItemAttributeHoldableActive : ItemAttributeHoldable
    {

    }
    public class ItemAttributeUnderground : IItemAttribute
    {

    }
    public class ItemAttributeBonusExp : IItemAttribute
    {

    }
    public class ItemAttributePreventsEvolution : IItemAttribute
    {

    }
    public class ItemAttributePassDownNature : IItemAttribute
    {

    }
}
