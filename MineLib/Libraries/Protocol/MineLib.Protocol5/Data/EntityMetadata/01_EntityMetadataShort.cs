using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Data.EntityMetadata
{
    /// <summary>
    /// Short Metadata
    /// </summary>
    public class EntityMetadataShort : EntityMetadataEntry
    {
        protected override byte Identifier => 1;
        protected override string FriendlyName => "short";

        public short Value;

        public static implicit operator EntityMetadataShort(short value) => new EntityMetadataShort(value);

        public EntityMetadataShort() { }
        public EntityMetadataShort(short value) { Value = value; }

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