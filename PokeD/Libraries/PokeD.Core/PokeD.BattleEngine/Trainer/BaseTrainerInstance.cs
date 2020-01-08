using PokeD.BattleEngine.Item;
using PokeD.BattleEngine.Trainer.Data;
using PokeD.BattleEngine.Trainer.Enums;

namespace PokeD.BattleEngine.Trainer
{
    public abstract class BaseTrainerInstance : IItemContainer
    {
        public ITrainerStaticData StaticData { get; }

        public virtual int ID { get; }
        public virtual string Name { get; }

        public virtual Gender Gender { get; }

        public virtual MonsterTeam Team { get; }

        protected virtual IItemContainer Bag { get; } = new Bag();


        public BaseTrainerInstance(ITrainerStaticData staticData)
        {
            StaticData = staticData;
        }


        public bool AttachItem(BaseItemInstance item) => Bag.AttachItem(item);
        public bool RemoveItem(BaseItemInstance item) => Bag.RemoveItem(item);
        public bool HoldsItem(IItemStaticData item) => Bag.HoldsItem(item);
    }
}