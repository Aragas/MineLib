using MineLib.Protocol.Netty.Packets;

using System;

namespace MineLib.Protocol.Netty.Protocol
{
    public sealed class StatusEnumFactory<TEnum> : ProtocolNettyFactory<ProtocolNettyPacket<TEnum>, TEnum> where TEnum : Enum { }
}