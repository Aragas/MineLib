using Aragas.Network.Data;
using Aragas.Network.Packets;

using MineLib.Protocol.Netty.Packets;
using MineLib.Protocol.Netty.Packets.Client;
using MineLib.Protocol.Netty.Packets.Server;
using MineLib.Protocol.Packets;

using System;

namespace MineLib.Protocol.Netty.Protocol
{
    public class PlayEnumFactory<TEnum> : ProtocolNettyFactory<ProtocolNettyPacket<TEnum>, TEnum> where TEnum : Enum { }

    public class PlayFactory<TPacket> : DefaultPacketFactory<TPacket, VarInt> where TPacket : MinecraftPacket { }

    public class ServerPlayFactory : DefaultPacketFactory<ServerPlayPacket, VarInt> { }
    public class ClientPlayFactory : DefaultPacketFactory<ClientPlayPacket, VarInt> { }
}