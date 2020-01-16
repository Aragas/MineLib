using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Server.Proxy.Protocol.Netty.Packets;

namespace MineLib.Server.Proxy.Protocol.Netty.Factory
{
    internal sealed class StatusStateFactory : DefaultPacketFactory<StatusStatePacket, VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}