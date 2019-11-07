using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using Aragas.QServer.Core.Packets;

namespace Aragas.QServer.Core.Protocol
{
    public sealed class InternalFactory : DefaultPacketFactory<InternalPacket, VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}