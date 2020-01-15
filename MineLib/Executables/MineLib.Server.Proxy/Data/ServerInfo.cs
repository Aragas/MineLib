using System;
using System.Collections.Generic;
using System.Text;

namespace MineLib.Server.Proxy.Data
{
    public class ServerInfo
    {
        public int MaxConnections { get; set; } = 1000;
        public int CurrentConnections { get; set; }
        public string Description { get; set; } = "Scalable C# Server";
    }
}
