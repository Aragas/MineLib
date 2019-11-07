using System;

namespace Aragas.QServer.MBus
{
    internal sealed class MBusClientMessageReceivedEventArgs : EventArgs
    {
        public Guid ClientGUID { get; set; }
        public byte[] Message { get; set; }

        public MBusClientMessageReceivedEventArgs(Guid clientGUID, byte[] message)
        {
            ClientGUID = clientGUID;
            Message = message;
        }
    }
}