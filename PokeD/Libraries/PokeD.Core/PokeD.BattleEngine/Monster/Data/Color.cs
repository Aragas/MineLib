namespace PokeD.BattleEngine.Monster.Data
{
    public class Color
    {
        public static Color None => new Color(0, "NONE");

        public int ID { get; }
        public string Name { get; }

        public Color(int id, string name) { ID = id; Name = name; }

        public override string ToString() => $"{Name}";
    }
}