using System;

namespace PokeD.BattleEngine.EggGroup
{
    public class EggGroups
    {
        public IEggGroupStaticData Type_0 { get; }
        public IEggGroupStaticData Type_1 { get; }

        public EggGroups(params IEggGroupStaticData[] eggGroups)
        {
            if (eggGroups.Length > 0)
                Type_0 = eggGroups[0];

            if (eggGroups.Length > 1)
                Type_1 = eggGroups[1];

            if (eggGroups.Length > 2)
                throw new ArgumentOutOfRangeException();
        }

        public override string ToString() => Equals(Type_1, null) ? $"{Type_0}, {Type_1}" : $"{Type_0}";


        public bool Contains(IEggGroupStaticData eggGroup) => Type_0 == eggGroup || Type_1 == eggGroup;
        public bool Contains(byte eggGroupID) => Type_0?.ID == eggGroupID || Type_1?.ID == eggGroupID;
    }
}