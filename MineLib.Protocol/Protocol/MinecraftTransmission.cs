using Aragas.Network.IO;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol.Protocol
{
    /// <summary>
    /// Standard Factory property is disabled
    /// </summary>
    public class MinecraftTransmission : ProtobufTransmission<MinecraftPacket>
    {
        //public MinecraftTransmission(Socket socket) : base(socket, null, new EmptyFactory()) { }
        public MinecraftTransmission() : base() { }

        public bool TryReadPacket(out MinecraftPacket? packet)
        {
            packet = ReadPacket();
            return packet != null;
        }
    }
}