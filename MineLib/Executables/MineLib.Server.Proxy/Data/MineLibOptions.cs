namespace MineLib.Server.Proxy.Data
{
    public class MineLibOptions
    {
        public string Name { get; set; } = "MineLib";
        public string Description { get; set; } = "Scalable C# Server";
        public int MaxConnections { get; set; } = 1000;
        public bool NettyLegacyPingEnable { get; set; } = true;
        public int NettyLegacyPingProtocol { get; set; } = 62;
    }
}