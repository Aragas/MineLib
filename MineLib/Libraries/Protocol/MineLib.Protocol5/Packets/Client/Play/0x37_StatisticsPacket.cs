using Aragas.Network.IO;
using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class StatisticsPacket : ClientPlayPacket
    {
        public StatisticsEntry[] Entry;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Entry = deserializer.Read(Entry);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Entry);
        }
    }
}