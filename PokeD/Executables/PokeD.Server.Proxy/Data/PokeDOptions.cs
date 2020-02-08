namespace PokeD.Server.Proxy.Data
{
    public class PokeDOptions
    {
        public string Name { get; set; } = "PokeD Docker";
        public string Description { get; set; } = "Scalable C# Server";
        public int MaxConnections { get; set; } = 1000;
    }
}