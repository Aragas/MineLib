using System.Collections.Generic;
using System.Linq;

using PokeD.BattleEngine.Ability;
using PokeD.BattleEngine.Attack;
using PokeD.BattleEngine.Item;
using PokeD.BattleEngine.Item.Data;
using PokeD.BattleEngine.Monster.Calculation;
using PokeD.BattleEngine.Monster.Data;
using PokeD.BattleEngine.Monster.Enums;
using PokeD.BattleEngine.Trainer;

namespace PokeD.BattleEngine.Monster
{
    public abstract class BaseMonsterInstance : IItemContainer
    {
        public IMonsterStaticData StaticData { get; }

        protected BaseTrainerInstance Owner { get; }


        public virtual CatchInfo CatchInfo { get; protected set; } = CatchInfo.None;

        public virtual Gender Gender { get; protected set; }

        public virtual BaseAbility Ability { get; protected set; }
        public virtual byte Nature { get; protected set; }

        public virtual long Experience { get; protected set; }
        public virtual byte Level => ExperienceCalculator.LevelForExperienceValue(StaticData.ExperienceType, Experience);

        public virtual Stats IV { get; protected set; } = Stats.None;
        public virtual Stats EV { get; protected set; } = Stats.None;

        public virtual short CurrentHP { get; protected set; }
        public virtual byte StatusEffect { get; protected set; }

        public virtual byte Affection { get; protected set; }
        public virtual byte Friendship { get; protected set; }

        public virtual bool IsShiny { get; protected set; }

        public virtual int EggSteps { get; protected set; }

        public virtual IList<BaseAttackInstance> Moves { get; protected set; } = new List<BaseAttackInstance>();

        public virtual BaseItemInstance HeldItem { get; protected set; }

        public BaseMonsterInstance(IMonsterStaticData staticData) { StaticData = staticData; }


        public bool AttachItem(BaseItemInstance item)
        {
            if (item.StaticData.Attributes.Any(att => att is ItemAttributeHoldable))
                return false;

            if (HeldItem != null)
            {
                Owner.AttachItem(HeldItem);
                HeldItem = item;
            }
            else
                HeldItem = item;

            return true;
        }
        public bool RemoveItem(BaseItemInstance item)
        {
            HeldItem = null;
            return true;
        }
        public bool HoldsItem(IItemStaticData item) => HeldItem.StaticData == item;
    }
}
