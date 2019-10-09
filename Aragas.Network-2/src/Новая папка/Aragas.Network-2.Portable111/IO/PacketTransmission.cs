using System;
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
        protected PacketStreamTransmission(Stream stream, BasePacketFactory<TPacketType, TPacketIDType, TSerializer, TDeserializer> factory)
        {
            Stream = stream;
            Factory = factory ?? new DefaultPacketFactory<TPacketType, TPacketIDType, TSerializer, TDeserializer>();
        }
    }

    public abstract class PacketTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer> : PacketTransmission, IDisposable
        where TPacketType : Packet<TPacketIDType, TSerializer, TDeserializer> where TSerializer : PacketSerializer where TDeserializer : PacketDeserializer
    {
        protected internal BasePacketFactory<TPacketType, TPacketIDType, TSerializer, TDeserializer> Factory { get; internal set; }

        protected PacketTransmission() { }
        protected PacketTransmission(BasePacketFactory<TPacketType, TPacketIDType, TSerializer, TDeserializer> factory) 
            => Factory = factory ?? new DefaultPacketFactory<TPacketType, TPacketIDType, TSerializer, TDeserializer>();


        public abstract void SendPacket(TPacketType packet);
        public abstract TPacketType ReadPacket();

        public abstract void Dispose();
    }

    public abstract class PacketTransmission { }
}