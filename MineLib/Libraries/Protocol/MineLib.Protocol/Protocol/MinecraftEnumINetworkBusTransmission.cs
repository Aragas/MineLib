using Aragas.QServer.Core.Protocol;
using Aragas.QServer.NetworkBus;

using MineLib.Protocol.Packets;

using System;

namespace MineLib.Protocol.Protocol
{
    public class MinecraftEnumINetworkBusTransmission : ProtobufINetworkBusTransmission<MinecraftEnumPacket>
    {
        public MinecraftEnumINetworkBusTransmission(IAsyncNetworkBus networkBus, Guid playerId) : base(networkBus, playerId) { }
    }
}