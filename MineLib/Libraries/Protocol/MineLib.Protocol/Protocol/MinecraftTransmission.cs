using Aragas.Network.IO;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol.Protocol
{

    /// <summary>
    /// Standard Factory property is disabled
    /// </summary>
    public class MinecraftTransmission : ProtobufTransmission<MinecraftPacket>
    {
        public MinecraftTransmission() : base() { }
    }
}