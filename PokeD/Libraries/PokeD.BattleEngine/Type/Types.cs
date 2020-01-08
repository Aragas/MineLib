using System;

namespace PokeD.BattleEngine.Type
{
    public class Types
    {
        public ITypeStaticData Type_0 { get; }
        public ITypeStaticData Type_1 { get; }

        public Types(params ITypeStaticData[] types)
        {
            if (types.Length > 0)
                Type_0 = types[0];

            if (types.Length > 1)
                Type_1 = types[1];

            if (types.Length > 2)
                throw new Exception();
        }

        public override string ToString() => ReferenceEquals(Type_1, null) ? $"{Type_0}" : $"{Type_0}, {Type_1}";


        public bool Contains(ITypeStaticData type) => Type_0 == type || Type_1 == type;
        public bool Contains(byte type) => Type_0?.ID == type || Type_1?.ID == type;
    }
}
