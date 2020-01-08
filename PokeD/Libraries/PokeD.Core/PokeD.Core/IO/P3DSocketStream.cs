using System;
using System.IO;
using System.Net.Sockets;

namespace PokeD.Core.IO
{
    public class P3DSocketStream : NetworkStream
    {
        public P3DSocketStream(Socket socket) : base(socket)
        {
            socket.Blocking = false;
            socket.ReceiveTimeout = 500;
            socket.SendTimeout = 500;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!Socket.Connected)
                return;

            try { base.Write(buffer, offset, count); }
            catch (Exception e) when (e is IOException || e is SocketException) { }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!Socket.Connected)
                return 0;

            try { return base.Read(buffer, offset, count); }
            catch (Exception e) when (e is IOException || e is SocketException) { return 0; }
        }

        private readonly byte[] _byteBuffer = new byte[1];
        public override int ReadByte()
        {
            return Read(_byteBuffer, 0, 1) != 0 ? _byteBuffer[0] : -1;
        }
    }
}