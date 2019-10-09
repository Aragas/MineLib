using System;

namespace MineLib.Protocol.Server
{
    public abstract class ProtocolConnection : IDisposable
    {
        public abstract string Name { get; }
        public abstract string Version { get; }
        public abstract int NetworkVersion { get; }

        public abstract string Host { get; }
        public abstract ushort Port { get; }
        public abstract bool Connected { get; }
        
        public abstract void Disconnect();

        public abstract void Dispose();
    }
}