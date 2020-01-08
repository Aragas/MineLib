using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using PokeD.BattleEngine.Battle.Data;

namespace PokeD.BattleEngine.Battle
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EnabledSidesAttribute : Attribute
    {
        public BattleSides Option { get; }

        public EnabledSidesAttribute(BattleSides option) { Option = option; }

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EnabledTrainersBySideAttribute : Attribute
    {
        public BattleTrainersBySide Option { get; }

        public EnabledTrainersBySideAttribute(BattleTrainersBySide option) { Option = option; }

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EnabledMonstersByTrainerAttribute : Attribute
    {
        public BattleMonstersByTrainer Option { get; }

        public EnabledMonstersByTrainerAttribute(BattleMonstersByTrainer option) { Option = option; }
    }



    //public interface IUpdatable { void Update(); }
    public enum BattleState { WaitingForTrainers, Started, Ended }

    // -- Guess the best way is to store Trainer class and operate mostly with it's ID.
    public abstract class Battle
    {
        #region Events
        private OrderedInvocationList _BattleStarted;
        protected event Action BattleStarted { add { _BattleStarted.Add(value); } remove { _BattleStarted.Remove(value); } }

        private OrderedInvocationList _BattleEnded;
        protected event Action BattleEnded { add { _BattleEnded.Add(value); } remove { _BattleEnded.Remove(value); } }


        private OrderedInvocationList<bool> _TurnReady;
        protected event Action<bool> TurnReady { add { _TurnReady.Add(value); } remove { _TurnReady.Remove(value); } }

        private OrderedInvocationList _TurnCompleted;
        protected event Action TurnCompleted { add { _TurnCompleted.Add(value); } remove { _TurnCompleted.Remove(value); } }


        private OrderedInvocationList<Weather> _WeatherChanged;
        protected event Action<Weather> WeatherChanged { add { _WeatherChanged.Add(value); } remove { _WeatherChanged.Remove(value); } }
        #endregion Events


        #region Values
        public BattleSides Sides { get; }
        public BattleTrainersBySide TrainersBySide { get; }
        public BattleMonstersByTrainer MonstersByTrainer { get; }

        private Weather _weather;
        protected Weather Weather
        {
            get { return _weather; }
            set
            {
                _weather = value;
                _WeatherChanged.Invoke(_weather); // -- Should't be executed in same thread.
            }
        }

        protected TrainerList Trainers;

        protected Dictionary<int, Turn> Turns { get; } = new Dictionary<int, Turn>(); // -- Expand Trainer class with Turn class.
        protected bool TrainersReady => Turns.All(pair => pair.Value != null);

        protected BattleState BattleState { get; set; } = BattleState.WaitingForTrainers;

        protected DateTime LastTurn { get; set; } = DateTime.Now;
        #endregion Values



        public Battle(BattleSides sides, BattleTrainersBySide trainersBySide, BattleMonstersByTrainer monstersByTrainer, bool enableExperience = true)
        {
            if (!GetType().GetTypeInfo().GetCustomAttribute<EnabledSidesAttribute>().Option.HasFlag(sides))
                throw new NotSupportedException();
            if (!GetType().GetTypeInfo().GetCustomAttribute<EnabledTrainersBySideAttribute>().Option.HasFlag(trainersBySide))
                throw new NotSupportedException();
            if (!GetType().GetTypeInfo().GetCustomAttribute<EnabledMonstersByTrainerAttribute>().Option.HasFlag(monstersByTrainer))
                throw new NotSupportedException();

            Sides = sides;
            TrainersBySide = trainersBySide;
            MonstersByTrainer = monstersByTrainer;


            _BattleStarted.Add(() =>
            {
                BattleState = BattleState.Started;
            }, InvocationOrder.Begin);

            _TurnReady.Add((forced) =>
            {
                LastTurn = DateTime.Now;
            }, InvocationOrder.Begin);
            _TurnReady.Add(DoTurn);

            _TurnCompleted.Add(() =>
            {
                for (byte i = 0; i < Trainers.Sides * Trainers.Trainers; i++)
                    Trainers[i] = null; // -- Clear last Turn.
            }, InvocationOrder.Begin);
            _TurnCompleted.Add(() =>
            {
                if (BattleState == BattleState.Ended)
                    _BattleEnded.Invoke(); // -- Should't be executed in same thread.
            }, InvocationOrder.End);
        }


        public bool SetTrainers(TrainerList trainers)
        {
            if (BattleState != BattleState.WaitingForTrainers)
                return false;

            Trainers = trainers;

            
            _BattleStarted?.Invoke(); // -- Should't be executed in same thread.

            return true;
        }


        public bool SetTrainerTurn(Turn turn)
        {
            if (BattleState != BattleState.Started)
                return false;

            if (Turns[turn.OwnerID] != null)
                return false; // -- Turn was already set.

            Turns[turn.OwnerID] = turn;


            if (TrainersReady)
                _TurnReady?.Invoke(false); // -- Should't be executed in same thread.

            return true;
        }


        protected virtual void DoTurn(bool forced = false)
        {
            // ...
            // ...
            // ...
            // Calculate all Monsters initiative, add to them move priority 

            _TurnCompleted?.Invoke();
        }



        public void Update()
        {
            if (DateTime.Now - LastTurn > new TimeSpan(0, 0, 30))
                _TurnReady.Invoke(true); // -- Force a Turn.
        }
    }



    [EnabledSides(BattleSides.Two)]
    [EnabledTrainersBySide(BattleTrainersBySide.One)]
    [EnabledMonstersByTrainer(BattleMonstersByTrainer.One | BattleMonstersByTrainer.Two | BattleMonstersByTrainer.Three)]
    public class Battle1v1 : Battle
    {
        public Battle1v1(BattleMonstersByTrainer monstersByTrainer, bool enableExperience = true) : base(BattleSides.Two, BattleTrainersBySide.One, monstersByTrainer, enableExperience)
        {

        }

        protected override void DoTurn(bool forced = false)
        {



            base.DoTurn(forced);
        }
    }

    [EnabledSides(BattleSides.Two)]
    [EnabledTrainersBySide(BattleTrainersBySide.Two)]
    [EnabledMonstersByTrainer(BattleMonstersByTrainer.One | BattleMonstersByTrainer.Two)]
    public class Battle2v2 : Battle
    {
        public Battle2v2(BattleMonstersByTrainer monstersByTrainer, bool enableExperience = true) : base(BattleSides.Two, BattleTrainersBySide.Two, monstersByTrainer, enableExperience)
        {

        }
    }

    [EnabledSides(BattleSides.Two)]
    [EnabledTrainersBySide(BattleTrainersBySide.Three)]
    [EnabledMonstersByTrainer(BattleMonstersByTrainer.One)]
    public class Battle3v3 : Battle
    {
        public Battle3v3(bool enableExperience = true) : base(BattleSides.Two, BattleTrainersBySide.Two, BattleMonstersByTrainer.One, enableExperience)
        {

        }
    }

    [EnabledSides(BattleSides.Four)]
    [EnabledTrainersBySide(BattleTrainersBySide.One)]
    [EnabledMonstersByTrainer(BattleMonstersByTrainer.One | BattleMonstersByTrainer.Two)]
    public class Battle1v1v1v1 : Battle
    {
        public Battle1v1v1v1(BattleMonstersByTrainer monstersByTrainer, bool enableExperience = true) : base(BattleSides.Four, BattleTrainersBySide.One, monstersByTrainer, enableExperience)
        {

        }
    }

    [EnabledSides(BattleSides.Four)]
    [EnabledTrainersBySide(BattleTrainersBySide.One)]
    [EnabledMonstersByTrainer(BattleMonstersByTrainer.One)]
    public class Battle1v1v1v1v1v1 : Battle
    {
        public Battle1v1v1v1v1v1(bool enableExperience = true) : base(BattleSides.Four, BattleTrainersBySide.One, BattleMonstersByTrainer.One, enableExperience)
        {

        }
    }
}
