namespace PokeD.BattleEngine.Attack.Data
{
    public class AttackLearn
    {
        public interface IAttackLearnCondition { }
        public class ByLevel : IAttackLearnCondition
        {
            public byte Level { get; }

            public ByLevel(byte level) { Level = level; }

            public override string ToString() => $"By Level {Level:000}";
        }
        public class ByBreeding : IAttackLearnCondition
        {
            public ByBreeding() { }

            public override string ToString() => "By Breeding";
        }
        public class ByTutor : IAttackLearnCondition
        {
            public ByTutor() { }

            public override string ToString() => "By Tutor";
        }
        public class ByMachine : IAttackLearnCondition
        {
            public ByMachine() { }

            public override string ToString() => "By Machine";
        }


        public IAttackStaticData Attack { get; }

        public IAttackLearnCondition LearnCondition { get; }


        public AttackLearn(IAttackStaticData attack, IAttackLearnCondition learnCondition) { Attack = attack; LearnCondition = learnCondition; }

        public override string ToString() => $"{Attack} | {LearnCondition}";
    }
}