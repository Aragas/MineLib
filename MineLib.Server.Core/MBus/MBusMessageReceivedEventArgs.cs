using System;

namespace MineLib.Server.Core
{
    public class MBusMessageReceivedEventArgs : EventArgs
    {
#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] Message { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        public MBusMessageReceivedEventArgs(byte[] message) => Message = message;
    }
}