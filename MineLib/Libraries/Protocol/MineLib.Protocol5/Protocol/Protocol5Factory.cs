using System;

using MineLib.Protocol.Protocol;
using MineLib.Protocol5.Packets;

namespace MineLib.Protocol5.Protocol
{
    public class Protocol5Factory<TPacketType, TEnum> : MinecraftEnumFactory<TPacketType, TEnum> 
        where TPacketType : Protocol5Packet<TEnum>
        where TEnum : Enum { }
}