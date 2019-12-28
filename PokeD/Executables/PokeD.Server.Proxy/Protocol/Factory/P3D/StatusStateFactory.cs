using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using PokeD.Server.Proxy.Packets.P3D;

namespace PokeD.Server.Proxy.Protocol.Factory.P3D
{
    internal sealed class StatusStateFactory : DefaultPacketFactory<StatusStatePacket, VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}