using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Server.Core.Packets;

namespace MineLib.Server.Core.Protocol
{
    public sealed class InternalFactory : DefaultPacketFactory<InternalPacket, VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}