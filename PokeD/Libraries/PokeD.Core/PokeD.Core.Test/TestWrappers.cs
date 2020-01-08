using System.IO;

using PCLExt.Network;

namespace PokeD.Core.Test
{
    public class TestITCPClient : ITCPClient
    {
        public MemoryStream Stream { get; } = new MemoryStream();

        public IPPort LocalEndPoint { get; }
        public IPPort RemoteEndPoint { get; }
        public bool IsConnected => true;
        public int DataAvailable => (int) (Stream.Length - Stream.Position);
        

        public void Connect(string ip, ushort port) { }
        public void Disconnect() { }
        
        public void Write(byte[] buffer, int offset, int count) { Stream.Write(buffer, offset, count); }
        public int Read(byte[] buffer, int offset, int count) => Stream.Read(buffer, offset, count);


        public void Dispose() { }
    }
}
