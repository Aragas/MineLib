using Aragas.Network.IO;
using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class StatisticsPacket : ClientPlayPacket
    {
        public StatisticsEntry[] Entry;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
            Entry = deserialiser.Read(Entry);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Entry);
        }
    }
}