using MineLib.Protocol.Netty.Protocol;
using MineLib.Protocol5.Packets;

namespace MineLib.Protocol5.Server
{
    public class Protocol5Transmission : ProtocolNettyTransmission<ServerStatusPacketTypes, ServerLoginPacketTypes, ServerPlayPacketTypes>
    {
    }
}