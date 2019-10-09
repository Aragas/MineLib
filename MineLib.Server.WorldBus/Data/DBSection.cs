using LiteDB;

using MineLib.Core.Anvil;
using MineLib.Core.IO;

namespace MineLib.Server.WorldBus
{
    public sealed class DBSection
    {
        [BsonId]
        public long Location { get; set; } // Chunk X, Section Y, Chunk Z
        public byte[] SerializedSection { get; set; }

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