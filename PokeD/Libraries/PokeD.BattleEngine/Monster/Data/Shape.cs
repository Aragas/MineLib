namespace PokeD.BattleEngine.Monster.Data
{
    public class Shape
    {
        public static Shape None => new Shape(0, "NONE");

        public int ID { get; }
        public string Name { get; }

        public Shape(int id, string name) { ID = id; Name = name; }

        public override string ToString() => $"{Name}";
    }
}