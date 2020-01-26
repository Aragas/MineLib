using Aragas.Network.Data;
using Aragas.Network.Packets;

using MineLib.Protocol.Netty.Packets.Client;

namespace MineLib.Protocol.Netty.Protocol
{
    public sealed class ClientLoginFactory : DefaultPacketFactory<ClientLoginPacket, VarInt> { }
}