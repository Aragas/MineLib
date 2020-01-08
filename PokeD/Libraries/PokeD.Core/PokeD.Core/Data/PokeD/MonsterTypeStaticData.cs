using System.Linq;

using PokeAPI;

using PokeD.BattleEngine.Type;
using PokeD.BattleEngine.Type.Data;

namespace PokeD.Core.Data.PokeD
{
    public class MonsterTypeStaticData : ITypeStaticData
    {
        public byte ID { get; }
        public string Name { get; }

        public DamageRelationRole DamageRelations { get; } = new DamageRelationRole();

        public MonsterTypeStaticData(byte id)
        {
            var type = DataFetcher.GetApiObject<PokemonType>(id).Result;

            ID = id;
            Name = type.Names.Single(MonsterStaticData.GetLocalizedName).Name;

            foreach (var damageRelation in type.DamageRelations.DoubleDamageTo)
                DamageRelations.Attacker.Add(new TypeDamageRelationEffective((byte) damageRelation.ID));
            foreach (var damageRelation in type.DamageRelations.HalfDamageTo)
                DamageRelations.Attacker.Add(new TypeDamageRelationNotEffective((byte) damageRelation.ID));
            foreach (var damageRelation in type.DamageRelations.NoDamageTo)
                DamageRelations.Attacker.Add(new TypeDamageRelationNone((byte) damageRelation.ID));

            foreach (var damageRelation in type.DamageRelations.DoubleDamageFrom)
                DamageRelations.Defender.Add(new TypeDamageRelationEffective((byte) damageRelation.ID));
            foreach (var damageRelation in type.DamageRelations.HalfDamageFrom)
                DamageRelations.Defender.Add(new TypeDamageRelationNotEffective((byte) damageRelation.ID));
            foreach (var damageRelation in type.DamageRelations.NoDamageFrom)
                DamageRelations.Defender.Add(new TypeDamageRelationNone((byte) damageRelation.ID));
        }

        public override string ToString() => $"{Name}";
    }
}
