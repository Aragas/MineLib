using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Core.IO;
using MineLib.Server.Core.Packets;

using System;

namespace MineLib.Server.Core.Extensions
{
    public static class PacketFactoryExtension
    {
        public static InternalPacket GetPacket(this BasePacketFactory<InternalPacket, VarInt, ProtobufSerializer, ProtobufDeserializer> factory, in Span<byte> data)
        {
            using var deserializer = new CompressedProtobufDeserializer(in data);
            var id = deserializer.Read<VarInt>();
            var packet = factory.Create(id);
            packet.Deserialize(deserializer);
            return packet;
        }
    }
}