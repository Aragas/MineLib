using PokeD.BattleEngine.Trainer;

namespace PokeD.Core.Data.PokeD
{
    public class Trainer : BaseTrainerInstance
    {
        public override int ID => TrainerID + SecretID * 65536;

        public short TrainerID { get; }
        public short SecretID { get; }

        public Trainer(int id) : base(Cached<TrainerStaticData>.Get(id)) { }
    }
}
