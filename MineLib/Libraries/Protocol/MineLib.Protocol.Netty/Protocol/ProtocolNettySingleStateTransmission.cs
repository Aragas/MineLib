using System;

using MineLib.Protocol.Protocol;
using MineLib.Protocol.Netty.Packets;

namespace MineLib.Protocol.Netty.Protocol
{
    /// <summary>
    /// Handles a single State, for example, Play State.
    /// </summary>
    public class ProtocolNettySingleStateTransmission<TPacketType, TEnum> : MinecraftSingleStateTransmission<TPacketType>
        where TPacketType : ProtocolNettyPacket<TEnum>
        where TEnum : Enum { }
}