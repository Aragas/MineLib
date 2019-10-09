
using System;

namespace MineLib.Server.MBus
{
    internal sealed class MBusClientMessageReceivedEventArgs : EventArgs
    {
        public Guid ClientGUID { get; set; }
        public byte[] Message { get; set; }
    }
}
