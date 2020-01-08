using System.Collections.Generic;
using System.Linq;

using PokeAPI;

using PokeD.BattleEngine.Item;
using PokeD.BattleEngine.Item.Data;

namespace PokeD.Core.Data.PokeD
{
    public class ItemStaticData : IItemStaticData
    {
        public int ID { get; }
        public string Name { get; }

        public IReadOnlyList<IItemAttribute> Attributes { get; }


        public ItemStaticData(int id)
        {
            var item = DataFetcher.GetApiObject<PokeAPI.Item>(id).Result;
            var itemAttributes = item.Attributes.Select(att => att.GetObject().Result);


            ID = id;
            Name = item.Names.Single(MonsterStaticData.GetLocalizedName).Name;

            var attributes = new List<IItemAttribute>();
            foreach (var itemAttribute in itemAttributes)
            {
                switch (itemAttribute.ID)
                {
                    case 1:
                        attributes.Add(new ItemAttributeCountable());
                        break;

                    case 2:
                        attributes.Add(new ItemAttributeConsumable());
                        break;

                    case 3:
                        attributes.Add(new ItemAttributeUsableOverworld());
                        break;

                    case 4:
                        attributes.Add(new ItemAttributeUsableInBattle());
                        break;

                    case 5:
                        attributes.Add(new ItemAttributeHoldable());
                        break;

                    case 6:
                        attributes.Add(new ItemAttributeHoldablePassive());
                        break;

                    case 7:
                        attributes.Add(new ItemAttributeHoldableActive());
                        break;

                    case 8:
                        attributes.Add(new ItemAttributeUnderground());
                        break;
                }
            }
            Attributes = attributes;
        }

        public override string ToString() => $"{Name}"; //$"{Name,-15}";
    }
}