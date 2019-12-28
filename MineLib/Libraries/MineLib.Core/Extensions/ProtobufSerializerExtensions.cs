using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using System;

namespace MineLib.Core.Extensions
{
    public static class ProtobufSerializerExtensions
    {
        public static ReadOnlySpan<byte> GetRawPacket<TSerializer, TDeserializer>(this TSerializer serializer, Packet<VarInt, TSerializer, TDeserializer> packet)
            where TSerializer : ProtobufSerializer
            where TDeserializer : PacketDeserializer
        {
            serializer.Write(packet.ID);
            packet.Serialize(serializer);
            return serializer.GetData();
        }
    }
}