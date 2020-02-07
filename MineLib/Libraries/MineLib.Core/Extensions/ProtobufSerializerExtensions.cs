using Aragas.Network.Data;
using Aragas.Network.Packets;

using System;

namespace Aragas.Network.IO
{
    public static class ProtobufSerializerExtensions
    {
        public static ReadOnlySpan<byte> GetRawPacket<TSerializer>(this TSerializer serializer, Packet<VarInt> packet)
            where TSerializer : ProtobufSerializer
        {
            serializer.Write(packet.ID);
            packet.Serialize(serializer);
            return serializer.GetData();
        }
    }
}