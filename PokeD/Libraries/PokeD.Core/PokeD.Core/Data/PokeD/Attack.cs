using PokeD.BattleEngine.Attack;

namespace PokeD.Core.Data.PokeD
{
    public class Attack : BaseAttackInstance
    {
        public Attack(short id) : base(Cached<AttackStaticData>.Get(id)) { }
        public Attack(short id, byte ppCurrent, byte ppUps) : base(Cached<AttackStaticData>.Get(id), ppCurrent, ppUps) { }

        public Attack(IAttackStaticData staticData) : base(staticData) { }
        public Attack(IAttackStaticData staticData, byte ppCurrent, byte ppUps) : base(staticData, ppCurrent, ppUps) { }
    }
}
