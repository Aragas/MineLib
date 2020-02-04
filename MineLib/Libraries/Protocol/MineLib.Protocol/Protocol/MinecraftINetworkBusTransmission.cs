using Aragas.Network.Data;
using Aragas.Network.Packets;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.Protocol;

using MineLib.Protocol.Packets;

using System;

namespace MineLib.Protocol.Protocol
{
    public class MinecraftINetworkBusTransmission : ProtobufINetworkBusTransmission<MinecraftPacket>
    {
        public MinecraftINetworkBusTransmission(IAsyncNetworkBus networkBus, Guid playerId, BasePacketFactory<MinecraftPacket, VarInt>? factory = null) : base(networkBus, playerId, factory) { }
    }
}