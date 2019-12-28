using System;

namespace Aragas.Network.IO
{
    public abstract class StreamSerializer : PacketSerializer
    {
        public abstract Span<byte> GetData();
    }
}