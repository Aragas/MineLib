using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Data;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class StatisticsPacket : ProtobufPacket
    {
        public StatisticsEntry[] Entry;

        public override VarInt ID => ClientPlayPacketTypes.Statistics;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
            Entry = reader.Read(Entry);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
            stream.Write(Entry);

            return this;
        }

    }
}