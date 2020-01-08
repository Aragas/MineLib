using System.Collections.Generic;

using PokeD.BattleEngine.Ability;
using PokeD.BattleEngine.Attack.Data;
using PokeD.BattleEngine.EggGroup;
using PokeD.BattleEngine.Item;
using PokeD.BattleEngine.Monster.Data;
using PokeD.BattleEngine.Monster.Enums;
using PokeD.BattleEngine.Type;

namespace PokeD.BattleEngine.Monster
{
    public interface IMonsterStaticData
    {
        short ID { get; }
        string Name { get; }

        int Height { get; }
        int Weight { get; }

        Types Types { get; }
        EggGroups EggGroups { get; }

        Stats BaseStats { get; }
        byte BaseHappiness { get; }

        Abilities Abilities { get; }

        IReadOnlyList<IItemStaticData> HeldItems { get; }

        byte CatchRate { get; }
        //byte EscapeRate { get; }

        float MaleRatio { get; }

        int HatchCycles { get; }

        bool IsBaby { get; }

        bool HasGenderDifferences { get; }

        Color Color { get; }
        Shape Shape { get; }

        Habitat Habitat { get; }

        ExperienceType ExperienceType { get; }

        short RewardExperience { get; }
        Stats RewardStats { get; }

        IReadOnlyList<EvolvesTo> EvolvesTo { get; }

        IReadOnlyList<AttackLearn> LearnableAttacks { get; }
    }
}
