using System;
using System.IO;
using System.Net.Sockets;

namespace Aragas.Network.IO
{ 
    public class SocketClientStream : Stream
    {
        public override bool CanRead { get; }
        public override bool CanSeek { get; }
        public override bool CanWrite { get; }
        public override long Length { get; }
        public override long Position { get; set; }

        private readonly Socket _client;
        
        public SocketClientStream(Socket client) { _client = client; }
        
        public override void Write(byte[] buffer, int offset, int count)
        {
            try { _client.Send(buffer, offset, count, SocketFlags.None); }
            catch (Exception e) when (e is SocketException) { }
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            try { return _client.Receive(buffer, offset, count, SocketFlags.None); }
            catch (Exception e) when (e is SocketException) { return -1; }
        }

        public override void Flush() => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
    }
}