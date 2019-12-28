using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Protocol.Packets
{
    public abstract class MinecraftPacket : Packet<VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}