using System.Linq;

using PokeAPI;

using PokeD.BattleEngine.Attack;
using PokeD.BattleEngine.Attack.Enums;
using PokeD.BattleEngine.Type;

namespace PokeD.Core.Data.PokeD
{
    public class AttackStaticData : IAttackStaticData
    {
        public short ID { get; }
        public string Name { get; }
        public ITypeStaticData Type { get; }

        public Target Target { get; }
        public DamageClass DamageClass { get; }

        public Condition AttackCondition { get; }

        public byte Power { get; }
        public byte Accuracy { get; }
        public byte Priority { get; }
        public byte PP { get; }

        public AttackStaticData(short id)
        {
            var move = DataFetcher.GetApiObject<Move>(id).Result;

            ID = id;
            Name = move.Names.Single(MonsterStaticData.GetLocalizedName).Name;
            Type = Cached<MonsterTypeStaticData>.Get((byte) move.Type.ID);

            Target = (Target) move.Target.ID;

            DamageClass = (DamageClass) move.DamageClass.ID;

            Power = (byte) (move.Power ?? 0);
            Accuracy = (byte) (move.Accuracy ?? 0);
            Priority = (byte) move.Priority;

            PP = (byte) (move.PP ?? 0);
        }

        public override string ToString() => $"[Name: {Name,-20} PP: {PP,2} Pow: {Power,3} Acc: {Accuracy,3} Type: {Type,-10}]";
    }
}