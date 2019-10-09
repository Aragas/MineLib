using System.Net.Sockets;

namespace MineLib.Server.Core
{
    public abstract class InternalListener
    {
        public abstract int Port { get; }
        protected TcpListener Listener { get; set; }

        public abstract void Start();
        public abstract void Stop();
    }
}