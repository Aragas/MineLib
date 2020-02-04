using Aragas.QServer.Core.NetworkBus;

using MineLib.Protocol.Netty;
using MineLib.Protocol.Netty.Protocol;
using MineLib.Protocol5.Packets;

using System;

namespace MineLib.Protocol5.Server
{
    public class Protocol5Transmission : ProtocolNettyEnumTransmission<ServerStatusPacketTypes, ServerLoginPacketTypes, ServerPlayPacketTypes>
    {
        public Protocol5Transmission(IAsyncNetworkBus networkBus, Guid playerId, State state) : base(networkBus, playerId, state) { }
    }
}