using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace Aragas.QServer.Core.Protocol
{
    public abstract class ProtobufINetworkBusTransmission<TProtobufPacketType> : SocketPacketINetworkBusTransmission<TProtobufPacketType, VarInt, ProtobufSerializer, ProtobufDeserializer>
        where TProtobufPacketType : Packet<VarInt>
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        protected ProtobufINetworkBusTransmission() : base() { }
    }
}
