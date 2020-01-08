using System.Collections.Generic;

namespace PokeD.BattleEngine.Type.Data
{
    public class DamageRelationRole
    {
        public IList<ITypeDamageRelation> Attacker { get; } = new List<ITypeDamageRelation>();
        public IList<ITypeDamageRelation> Defender { get; } = new List<ITypeDamageRelation>();
    }

    public interface ITypeDamageRelation
    {
        byte TypeID { get; }
        float Modifier { get; }
    }
    public class TypeDamageRelationNone : ITypeDamageRelation
    {
        public byte TypeID { get; }
        public float Modifier => 0F;

        public TypeDamageRelationNone(byte typeID) { TypeID = typeID; }
    }
    public class TypeDamageRelationNormal : ITypeDamageRelation
    {
        public byte TypeID { get; }
        public float Modifier => 1F;

        public TypeDamageRelationNormal(byte typeID) { TypeID = typeID; }
    }
    public class TypeDamageRelationEffective : ITypeDamageRelation
    {
        public byte TypeID { get; }
        public float Modifier => 2F;

        public TypeDamageRelationEffective(byte typeID) { TypeID = typeID; }
    }
    public class TypeDamageRelationNotEffective : ITypeDamageRelation
    {
        public byte TypeID { get; }
        public float Modifier => 0.5F;

        public TypeDamageRelationNotEffective(byte typeID) { TypeID = typeID; }
    }


}
