using MineLib.Protocol.Netty.Protocol;
using MineLib.Protocol5.Packets;

namespace MineLib.Protocol5.Server
{
    public class Protocol5Transmission : ProtocolNettyEnumTransmission<ServerStatusPacketTypes, ServerLoginPacketTypes, ServerPlayPacketTypes>
    {
    }
}