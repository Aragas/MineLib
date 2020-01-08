using System.Linq;

using PokeAPI;

using PokeD.BattleEngine.EggGroup;

namespace PokeD.Core.Data.PokeD
{
    public class EggGroupStaticData : IEggGroupStaticData
    {
        public byte ID { get; }
        public string Name { get; }

        public EggGroupStaticData(byte id)
        {
            var eggGroup = DataFetcher.GetApiObject<EggGroup>(id).Result;


            ID = id;
            Name = eggGroup.Names.Single(MonsterStaticData.GetLocalizedName).Name;
        }

        public override string ToString() => $"{Name}";
    }
}
