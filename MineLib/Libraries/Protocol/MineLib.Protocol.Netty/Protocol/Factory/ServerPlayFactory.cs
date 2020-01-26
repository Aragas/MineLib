using Aragas.Network.Data;
using Aragas.Network.Packets;

using MineLib.Protocol.Netty.Packets.Server;

namespace MineLib.Protocol.Netty.Protocol
{
    public class ServerPlayFactory : DefaultPacketFactory<ServerPlayPacket, VarInt> { }
}