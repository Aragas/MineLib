using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace PokeD.Core.Extensions
{
    public static class PacketTransmissionExtension
    {
        public static bool TryReadPacket<TPacket, TIDType>(this PacketTransmission<TPacket, TIDType> transmission, out TPacket packet)
            where TPacket : Packet<TIDType>
        {
            packet = transmission.ReadPacket();
            return packet != null;
        }
    }
}