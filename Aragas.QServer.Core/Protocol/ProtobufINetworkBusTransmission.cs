using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;
using Aragas.QServer.Core.NetworkBus;

using System;

namespace Aragas.QServer.Core.Protocol
{
    public abstract class ProtobufINetworkBusTransmission<TProtobufPacketType> : SocketPacketINetworkBusTransmission<TProtobufPacketType, VarInt, ProtobufSerializer, ProtobufDeserializer>
        where TProtobufPacketType : Packet<VarInt>
    {
        protected ProtobufINetworkBusTransmission(IAsyncNetworkBus networkBus, Guid playerId, BasePacketFactory<TProtobufPacketType, VarInt>? factory = null) : base(networkBus, playerId, factory) { }
    }
}