using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Data.EntityMetadata
{
    /// <summary>
    /// Int32 Metadata
    /// </summary>
    public class EntityMetadataInt : EntityMetadataEntry
    {
        protected override byte Identifier => 2;
        protected override string FriendlyName => "int";

        public int Value;

        public static implicit operator EntityMetadataInt(int value) => new EntityMetadataInt(value);

        public EntityMetadataInt() { }
        public EntityMetadataInt(int value) { Value = value; }

        public override void FromDeserializer(IPacketDeserializer deserializer)
        {
            Value = deserializer.Read(Value);
        }
        public override void ToSerializer(IPacketSerializer serializer, byte index)
        {
            serializer.Write(GetKey(index));
            serializer.Write(Value);
        }
    }
}