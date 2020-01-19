using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Protocol.Packets
{
    public abstract class MinecraftEnumPacket : Packet<VarInt, ProtobufSerializer, ProtobufDeserializer> { }
    public abstract class MinecraftPacket : PacketWithAttribute<VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}