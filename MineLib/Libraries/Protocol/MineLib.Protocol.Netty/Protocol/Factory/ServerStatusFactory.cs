using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Protocol.Netty.Packets.Server;

namespace MineLib.Protocol.Netty.Protocol
{
    public sealed class ServerStatusFactory : DefaultPacketFactory<ServerStatusPacket, VarInt> { }
}