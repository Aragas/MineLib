namespace PokeD.BattleEngine.Monster.Data
{
    public class Habitat
    {
        public static Habitat None => new Habitat(0, "NONE");

        public int ID { get; }
        public string Name { get; }

        public Habitat(int id, string name) { ID = id; Name = name; }

        public override string ToString() => $"{Name}";
    }
}