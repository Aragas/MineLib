using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Core.Anvil;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x39)]
    public sealed class SectionResponse : InternalPacket
    {
        public Section? Section;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Section = deserializer.Read(Section);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Section);
        }
    }
}