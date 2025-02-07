﻿using Aragas.Network.Data;
using Aragas.Network.Packets;

using MineLib.Protocol.Netty.Packets.Server;

namespace MineLib.Protocol.Netty.Protocol
{
    public sealed class ServerLoginFactory : DefaultPacketFactory<ServerLoginPacket, VarInt> { }
}