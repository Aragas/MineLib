using System;
using System.Linq;

namespace PokeD.BattleEngine.Ability
{
    public class Abilities
    {
        public BaseAbility Ability_0 { get; }
        public BaseAbility Ability_1 { get; }
        public BaseAbility Hidden { get; }

        public Abilities(params BaseAbility[] abilities)
        {
            abilities = abilities.OrderBy(ability => ability.StaticData.ID).ThenBy(ability => !ability.IsHidden).ToArray();

            if (abilities.Length > 0)
                Ability_0 = abilities[0];

            if (abilities.Length > 1)
            {
                if (!abilities[1].IsHidden)
                    Ability_1 = abilities[1];
                else
                    Hidden = abilities[1];
            }

            if (abilities.Length > 2)
                Hidden = abilities[2];

            if (abilities.Length > 3)
                throw new ArgumentOutOfRangeException();
        }

        public override string ToString()
        {
            var output = $"{Ability_0}";
            if (!ReferenceEquals(Ability_1, null))
                output += $", {Ability_1}";
            if (!ReferenceEquals(Hidden, null))
                output += $", {Hidden}";
            return output;
        }


        public bool Contains(BaseAbility ability) => Ability_0 == ability || Ability_1 == ability || Hidden == ability;
        public bool Contains(short abilityID) => Ability_0?.StaticData.ID == abilityID || Ability_1?.StaticData.ID == abilityID || Hidden?.StaticData.ID == abilityID;
    }
}