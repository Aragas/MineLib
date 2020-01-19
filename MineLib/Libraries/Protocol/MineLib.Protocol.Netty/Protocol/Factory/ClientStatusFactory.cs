using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Protocol.Netty.Packets.Client;

namespace MineLib.Protocol.Netty.Protocol
{
    public sealed class ClientStatusFactory : DefaultPacketFactory<ClientStatusPacket, VarInt> { }
}