using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Core.Extensions
{
    public static class PacketTransmissionExtension
    {
        public static bool TryReadPacket<TPacket, TIDType, TSerializer, TDeserializer>(this PacketTransmission<TPacket, TIDType> transmission, out TPacket packet)
            where TPacket : Packet<TIDType>
            => (packet = transmission.ReadPacket()) != null;
    }
}