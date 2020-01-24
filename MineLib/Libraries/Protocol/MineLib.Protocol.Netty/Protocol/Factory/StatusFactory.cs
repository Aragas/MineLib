using Aragas.Network.Data;
using Aragas.Network.Packets;

using MineLib.Protocol.Netty.Packets;
using MineLib.Protocol.Packets;

using System;

namespace MineLib.Protocol.Netty.Protocol
{
    public sealed class StatusEnumFactory<TEnum> : ProtocolNettyFactory<ProtocolNettyPacket<TEnum>, TEnum> where TEnum : Enum { }

    public sealed class StatusFactory<TPacket> : DefaultPacketFactory<TPacket, VarInt> where TPacket : MinecraftPacket { }
}