using Aragas.Core.IO;

using MineLib.Core.Data;

namespace ProtocolModern.Data.EntityMetadata
{
    /// <summary>
    /// Vector(Position) Metadata
    /// </summary>
    public class EntityMetadataVector : EntityMetadataEntry
    {
        protected override byte Identifier => 6;
        protected override string FriendlyName => "vector";

        public Position Value;

        public static implicit operator EntityMetadataVector(Position value) => new EntityMetadataVector(value);

        public EntityMetadataVector() { }
        public EntityMetadataVector(int x, int y, int z) { Value = new Position(x, y, z); }
        public EntityMetadataVector(Position value) { Value = value; }

        public override void FromReader(PacketDataReader reader)
        {
            Value = new Position(
                reader.Read<int>(),
                reader.Read<int>(),
                reader.Read<int>());
        }

        public override void ToStream(PacketStream stream, byte index)
        {
            stream.Write(GetKey(index));
            stream.Write(Value.X);
            stream.Write(Value.Y);
            stream.Write(Value.Z);
        }
    }
}
