/*
using Aragas.QServer.Core.IO;

using MineLib.Core.Anvil;

namespace MineLib.Server.WorldBus
{
    public sealed class DBSection
    {
        public long Location { get; set; } = default!; // Chunk X, Section Y, Chunk Z
        public byte[] SerializedSection { get; set; } = default!;

        public DBSection() { }
        public DBSection(in Section section)
        {
            Location = section.Location.GetDatabaseIndex();
            using var serializer = new CompressedProtobufSerializer();
            serializer.Write(section);
            SerializedSection = serializer.GetData().ToArray();
        }

        public Section ToSection()
        {
            using var deserialiser = new CompressedProtobufDeserializer(SerializedSection);
            return deserialiser.Read<Section>();
        }
    }
}
*/