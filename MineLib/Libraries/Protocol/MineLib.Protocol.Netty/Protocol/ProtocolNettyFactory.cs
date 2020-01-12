using System;

using MineLib.Protocol.Protocol;
using MineLib.Protocol.Netty.Packets;

namespace MineLib.Protocol.Netty.Protocol
{
    public class ProtocolNettyFactory<TPacketType, TEnum> : MinecraftEnumFactory<TPacketType> 
        where TPacketType : ProtocolNettyPacket<TEnum>
        where TEnum : Enum { }
}