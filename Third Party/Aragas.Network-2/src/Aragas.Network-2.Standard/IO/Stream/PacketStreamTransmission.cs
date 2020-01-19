using Aragas.Network.Packets;

using System.IO;

namespace Aragas.Network.IO
{
    public abstract class PacketStreamTransmission<TPacketType, TPacketIDType> : PacketTransmission<TPacketType, TPacketIDType>
        where TPacketType : Packet<TPacketIDType>
    {
        protected Stream Stream { get; set; } = Stream.Null;
        public virtual long AvailableData => Stream.Length - Stream.Position;

        protected PacketStreamTransmission() { }
        protected PacketStreamTransmission(Stream stream) { Stream = stream; }
    }
}