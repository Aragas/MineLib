using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Data.EntityMetadata
{
    /// <summary>
    /// Vector(Position) Metadata
    /// </summary>
    public class EntityMetadataVector : EntityMetadataEntry
    {
        protected override byte Identifier => 6;
        protected override string FriendlyName => "vector";

        public Location3D Value;

        public static implicit operator EntityMetadataVector(in Location3D value) => new EntityMetadataVector(value);

        public EntityMetadataVector() { }
        public EntityMetadataVector(int x, int y, int z) { Value = new Location3D(x, y, z); }
        public EntityMetadataVector(in Location3D value) { Value = value; }

        public override void FromDeserializer(PacketDeserializer deserializer)
        {
            Value = new Location3D(
                deserializer.Read<int>(),
                deserializer.Read<int>(),
                deserializer.Read<int>());
        }

        public override void ToSerializer(PacketSerializer serializer, byte index)
        {
            serializer.Write(GetKey(index));
            serializer.Write(Value.X);
            serializer.Write(Value.Y);
            serializer.Write(Value.Z);
        }
    }
}