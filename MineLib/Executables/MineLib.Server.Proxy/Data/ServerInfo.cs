using System;
using System.Text;

namespace MineLib.Server.Proxy.Data
{
    public class ServerInfo
    {
        //public string Name { get; set; } = default!;
        //public string Description { get; set; } = default!;
        //public int MaxConnections { get; set; } = default!;
        public int CurrentConnections { get; set; } = default!;
    }

    public class ClassicServerInfo
    {
        private const string SaltChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-.~";

        /// <summary> Secret string used to verify players' names.
        /// Randomly generated at startup, and can be randomized by "/reload salt"
        /// Known only to this server and to heartbeat server(s). </summary>
        public string Salt { get; set; } = default!;

        public ClassicServerInfo()
        {
            var rand = new Random();
            var sb = new StringBuilder(0);
            for (var i = 0; i < 16; i++)
                sb.Append(SaltChars[rand.Next(0, SaltChars.Length - 1)]);
            Salt = sb.ToString();
        }
    }
}