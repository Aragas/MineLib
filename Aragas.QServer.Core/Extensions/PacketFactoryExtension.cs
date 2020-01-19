using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;
using Aragas.QServer.Core.IO;
using Aragas.QServer.Core.Packets;

using System;

namespace Aragas.QServer.Core.Extensions
{
    public static class PacketFactoryExtension
    {
        public static InternalPacket? GetPacket(this BasePacketFactory<InternalPacket, VarInt> factory, in Span<byte> data)
        {
            using var deserializer = new CompressedProtobufDeserializer(in data);
            var id = deserializer.Read<VarInt>();
            var packet = factory.Create(id);
            if (packet != null)
            {
                packet.Deserialize(deserializer);
                return packet;
            }
            return null;
        }
    }
}