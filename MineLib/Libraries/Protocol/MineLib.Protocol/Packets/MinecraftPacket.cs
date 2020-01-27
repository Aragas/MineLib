using Aragas.Network.Data;
using Aragas.Network.Packets;

namespace MineLib.Protocol.Packets
{
    public abstract class MinecraftEnumPacket : Packet<VarInt> { }
    public abstract class MinecraftPacket : PacketWithIDAttribute<VarInt> { }
}