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
        /// <summary> Secret string used to verify players' names.
        /// Randomly generated at startup, and can be randomized by "/reload salt"
        /// Known only to this server and to heartbeat server(s). </summary>
        public string Salt { get; set; } = default!;

        public ClassicServerInfo()
        {
            const string SALT_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-.~";
            var rand = new Random();
            var sb = new StringBuilder(0);
            for (var i = 0; i < 16; i++)
                sb.Append(SALT_CHARS[rand.Next(0, SALT_CHARS.Length - 1)]);
            Salt = sb.ToString();
        }
    }
}