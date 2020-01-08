using PokeD.BattleEngine.Ability;

namespace PokeD.Core.Data.PokeD
{
    public class Ability : BaseAbility
    {
        public Ability(short id, bool isHidden) : base(Cached<AbilityStaticData>.Get(id), isHidden) { }
    }
}
