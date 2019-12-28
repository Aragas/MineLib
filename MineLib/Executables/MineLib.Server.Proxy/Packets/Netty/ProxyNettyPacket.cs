using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Server.Proxy.Packets.Netty
{
    public abstract class ProxyNettyPacket : PacketWithAttribute<VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}