using System.Collections.Generic;

using PokeD.BattleEngine.Monster.Data;

namespace PokeD.Core.Data.PokeD
{
    public class ByShed : EvolvesTo.IEvolutionCondition
    {
        public IReadOnlyList<EvolvesTo.ISubEvolutionCondition> SubConditions { get; } = new List<EvolvesTo.ISubEvolutionCondition>();

        public ByShed() { }

        public override string ToString() => $"By Shed";
    }
}