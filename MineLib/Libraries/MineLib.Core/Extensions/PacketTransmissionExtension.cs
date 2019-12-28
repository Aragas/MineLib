using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Core.Extensions
{
    public static class PacketTransmissionExtension
    {
        public static bool TryReadPacket<TPacket, TIDType, TSerializer, TDeserializer>(this PacketTransmission<TPacket, TIDType, TSerializer, TDeserializer> transmission, out TPacket packet)
            where TPacket : Packet<TIDType, TSerializer, TDeserializer>
            where TSerializer : PacketSerializer
            where TDeserializer : PacketDeserializer
            => (packet = transmission.ReadPacket()) != null;
    }
}