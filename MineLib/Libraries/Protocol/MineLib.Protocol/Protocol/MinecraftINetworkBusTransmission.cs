using Aragas.QServer.Core.Protocol;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol.Protocol
{
    public class MinecraftINetworkBusTransmission : ProtobufINetworkBusTransmission<MinecraftPacket>
    {
        public MinecraftINetworkBusTransmission() : base() { }
    }

    public class MinecraftEnumINetworkBusTransmission : ProtobufINetworkBusTransmission<MinecraftEnumPacket>
    {
        public MinecraftEnumINetworkBusTransmission() : base() { }
    }
}