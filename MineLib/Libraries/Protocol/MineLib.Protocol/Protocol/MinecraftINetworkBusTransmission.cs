using Aragas.QServer.Core.Protocol;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol.Protocol
{
    /// <summary>
    /// Standard Factory property is disabled
    /// </summary>
    public class MinecraftINetworkBusTransmission : ProtobufINetworkBusTransmission<MinecraftPacket>
    {
        public MinecraftINetworkBusTransmission() : base() { }
    }
}