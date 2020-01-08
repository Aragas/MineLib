using PokeD.BattleEngine.Attack;
using PokeD.BattleEngine.Item;
using PokeD.BattleEngine.Monster;

namespace PokeD.BattleEngine.Battle
{
    public enum TurnOption { Attack, Item, Switch, Flee }

    public abstract class Turn
    {
        public Battle Battle { get; }
        public int OwnerID { get; }
        public TurnOption Option { get; }


        public Turn(int ownerID, TurnOption option) { OwnerID = ownerID; Option = option; }
    }


    public class TurnFlee : Turn
    {
        public TurnFlee(int ownerID) : base(ownerID, TurnOption.Flee) { }
    }
    public class TurnSwitch : Turn
    {
        public TurnSwitch(int ownerID, BaseMonsterInstance toSwitch) : base(ownerID, TurnOption.Switch) { }
    }
    public class TurnItem : Turn
    {
        public TurnItem(int ownerID, BaseItemInstance item) : base(ownerID, TurnOption.Item) { }
    }
    public class TurnAttack : Turn
    {
        public TurnAttack(int ownerID, BaseAttackInstance attack, int targetID, int monsterID) : base(ownerID, TurnOption.Attack) { }
    }
}
