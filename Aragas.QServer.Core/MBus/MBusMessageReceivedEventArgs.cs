using System;

namespace Aragas.QServer.Core.MBus
{
    public class MBusMessageReceivedEventArgs : EventArgs
    {
        public byte[] Message { get; }

        public MBusMessageReceivedEventArgs(byte[] message)
        {
            Message = message;
        }
    }
}