using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Data.EntityMetadata
{
    /// <summary>
    /// Byte Metadata
    /// </summary>
    public class EntityMetadataByte : EntityMetadataEntry
    {
        protected override byte Identifier => 0;
        protected override string FriendlyName => "byte";

        public byte Value;

        public static implicit operator EntityMetadataByte(byte value) => new EntityMetadataByte(value);

        public EntityMetadataByte() { }
        public EntityMetadataByte(byte value) { Value = value; }

        public override void FromDeserializer(PacketDeserializer deserializer)
        {
            Value = deserializer.Read(Value);
        }
        public override void ToSerializer(PacketSerializer serializer, byte index)
        {
            serializer.Write(GetKey(index));
            serializer.Write(Value);
        }
    }
}