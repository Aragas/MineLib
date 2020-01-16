using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Server.Proxy.Protocol.Legacy.Packets
{
    internal abstract class ProxyLegacyPacket : PacketWithAttribute<VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}