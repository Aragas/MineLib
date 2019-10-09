using MineLib.Core.IO;
using MineLib.Server.Core.Packets;

namespace MineLib.Server.Core.Extensions
{
    public static class MBusExtensions
    {
        public static void SendPacket(this IMBus bus, InternalPacket packet)
        {
            using var serializer = new CompressedProtobufSerializer();
            serializer.Write(packet.ID);
            packet.Serialize(serializer);
            bus.SendMessage(serializer.GetData());
        }
    }
}