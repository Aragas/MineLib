using System.Collections;
using System.Collections.Generic;

using PokeD.BattleEngine.Monster;

namespace PokeD.BattleEngine.Trainer.Data
{
    public class MonsterTeam : IEnumerable<BaseMonsterInstance>
    {
        public List<BaseMonsterInstance> List { get; } = new List<BaseMonsterInstance>();

        public IEnumerator<BaseMonsterInstance> GetEnumerator() => List.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}