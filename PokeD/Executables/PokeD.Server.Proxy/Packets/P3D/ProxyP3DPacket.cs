using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace PokeD.Server.Proxy.Packets.P3D
{
    public abstract class ProxyP3DPacket : PacketWithAttribute<VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}