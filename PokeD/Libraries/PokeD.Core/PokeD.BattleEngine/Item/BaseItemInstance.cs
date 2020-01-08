namespace PokeD.BattleEngine.Item
{
    public abstract class BaseItemInstance
    {
        public IItemStaticData StaticData { get; }

        //public IItemContainer Holder { get; }

        public BaseItemInstance(IItemStaticData staticData) { StaticData = staticData; }
        //public BaseItemInstance(IItemStaticData staticData, IItemContainer holder) { StaticData = staticData; Holder = holder; }

        public override string ToString() => $"{StaticData}";
    }
}