using System;

using Aragas.Network.Packets;

namespace Aragas.Network.IO
{

    public abstract class PacketTransmission<TPacketType, TPacketIDType, TSerializer, TDeserializer> : IDisposable
        where TPacketType : Packet<TPacketIDType, TSerializer, TDeserializer>
        where TSerializer : PacketSerializer
        where TDeserializer : PacketDeserializer
    {
        protected PacketTransmission() { }

        public abstract void SendPacket(TPacketType packet);
        public abstract TPacketType ReadPacket();

        public abstract void Dispose();
    }
}