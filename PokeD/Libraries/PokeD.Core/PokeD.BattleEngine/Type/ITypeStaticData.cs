using PokeD.BattleEngine.Type.Data;

namespace PokeD.BattleEngine.Type
{
    public interface ITypeStaticData
    {
        byte ID { get; }
        string Name { get; }

        DamageRelationRole DamageRelations { get; }
    }
}
