﻿using MineLib.Protocol.Netty.Packets;

using System;

namespace MineLib.Protocol.Netty.Protocol
{
    public class PlayEnumFactory<TEnum> : ProtocolNettyFactory<ProtocolNettyPacket<TEnum>, TEnum> where TEnum : Enum { }
}