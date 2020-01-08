using System;

namespace MineLib.Protocol.Server
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Implementation problem")]
    public abstract class ProtocolConnection : IDisposable
    {
        public abstract string Name { get; }
        public abstract string Version { get; }
        public abstract int NetworkVersion { get; }
        public abstract int State { get; }

        public abstract string Host { get; }
        public abstract ushort Port { get; }
        public abstract bool Connected { get; }

        public abstract void Disconnect();

        public abstract void Dispose();
    }
}