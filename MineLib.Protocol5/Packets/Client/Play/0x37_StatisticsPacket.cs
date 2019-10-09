using Aragas.Network.IO;
using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class StatisticsPacket : ClientPlayPacket
    {
        public StatisticsEntry[] Entry;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            Entry = deserialiser.Read(Entry);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Entry);
        }
    }
}