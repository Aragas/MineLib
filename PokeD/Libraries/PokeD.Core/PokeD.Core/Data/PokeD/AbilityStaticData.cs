using System.Linq;

using PokeAPI;

using PokeD.BattleEngine.Ability;

namespace PokeD.Core.Data.PokeD
{
    public class AbilityStaticData : IAbilityStaticData
    {
        public short ID { get; }
        public string Name { get; }

        public AbilityStaticData(short id)
        {
            var ability = DataFetcher.GetApiObject<PokeAPI.Ability>(id).Result;


            ID = id;
            Name = ability.Names.Single(MonsterStaticData.GetLocalizedName).Name;
        }

        public override string ToString() => $"{Name}";
    }
}