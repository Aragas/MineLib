using System;

namespace MineLib.Server.Core
{
    public class MBusMessageReceivedEventArgs : EventArgs
    {
        public byte[] Message { get; set; }
    }
}