using PokeD.BattleEngine.Trainer;

namespace PokeD.Core.Data.PokeD
{
    public class TrainerStaticData : ITrainerStaticData
    {
        public int ID { get; }
        public string Name { get; }


        public TrainerStaticData(int id)
        {
            ID = id;
        }
    }
}