using Aragas.Network.Packets;

using System;

namespace Aragas.Network.IO
{

    public abstract class PacketTransmission<TPacketType, TPacketIDType> : IDisposable where TPacketType : Packet<TPacketIDType>
    {
        private bool disposedValue = false; // To detect redundant calls

        protected PacketTransmission() { }

        public abstract void SendPacket(TPacketType packet);
        public abstract TPacketType? ReadPacket();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }

        ~PacketTransmission()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}