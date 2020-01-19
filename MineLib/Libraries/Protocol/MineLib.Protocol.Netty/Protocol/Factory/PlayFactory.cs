using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Protocol.Netty.Packets;
using MineLib.Protocol.Packets;

using System;

namespace MineLib.Protocol.Netty.Protocol
{
    public class PlayEnumFactory<TEnum> : ProtocolNettyFactory<ProtocolNettyPacket<TEnum>, TEnum> where TEnum : Enum { }

    public class PlayFactory<TPacket> : DefaultPacketFactory<TPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
        where TPacket : MinecraftPacket { }
}