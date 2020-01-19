using System;

namespace Aragas.Network.IO
{
    public interface IStreamSerializer : IPacketSerializer
    {
        Span<byte> GetData();
    }
    public abstract class StreamSerializer : PacketSerializer, IStreamSerializer
    {
        public abstract Span<byte> GetData();
    }
}