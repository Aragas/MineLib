using System;

namespace Aragas.QServer.Core
{
    public class MBusMessageReceivedEventArgs : EventArgs
    {
        public byte[] Message { get; set; }

        public MBusMessageReceivedEventArgs(byte[] message) => Message = message;
    }
}