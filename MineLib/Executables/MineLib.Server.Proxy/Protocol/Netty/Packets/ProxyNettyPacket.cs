using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets
{
    public abstract class ProxyNettyPacket : PacketWithAttribute<VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}