using Aragas.Network.Data;
using Aragas.Network.Packets;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol.Netty.Protocol
{
    public class PlayFactory<TPacket> : DefaultPacketFactory<TPacket, VarInt> where TPacket : MinecraftPacket { }
}