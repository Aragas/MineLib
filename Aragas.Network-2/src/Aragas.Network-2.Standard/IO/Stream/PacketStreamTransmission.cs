using System.IO;

using Aragas.Network.Packets;

namespace Aragas.Network.IO
{
    public abstract class PacketStreamTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer> : PacketTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer>
        where TPacketType : Packet<TPacketIDType, TSerializer, TDeserializer>
        where TSerializer : PacketSerializer
        where TDeserializer : PacketDeserializer
    {
        protected Stream Stream { get; set; }
        public virtual long AvailableData => Stream.Length - Stream.Position;

        protected PacketStreamTransmission() { }
        protected PacketStreamTransmission(Stream stream) { Stream = stream; }
    }
}