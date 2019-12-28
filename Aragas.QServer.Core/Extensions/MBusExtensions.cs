using Aragas.QServer.Core.IO;
using Aragas.QServer.Core.Packets;

namespace Aragas.QServer.Core.Extensions
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