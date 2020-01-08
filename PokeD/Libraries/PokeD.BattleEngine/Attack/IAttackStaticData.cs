using PokeD.BattleEngine.Attack.Enums;
using PokeD.BattleEngine.Type;

namespace PokeD.BattleEngine.Attack
{
    public interface IAttackStaticData
    {
        short ID { get; }
        string Name { get; }
        ITypeStaticData Type { get; }

        Target Target { get; }
        DamageClass DamageClass { get; }
        Condition AttackCondition { get; }

        byte Power { get; }
        byte Accuracy { get; }
        byte Priority { get; }

        byte PP { get; }
    }
}