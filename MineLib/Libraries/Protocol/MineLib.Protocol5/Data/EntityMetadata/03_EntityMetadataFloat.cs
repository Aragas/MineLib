using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Data.EntityMetadata
{
    /// <summary>
    /// Float Metadata
    /// </summary>
    public class EntityMetadataFloat : EntityMetadataEntry
    {
        protected override byte Identifier => 3;
        protected override string FriendlyName => "float";

        public float Value;

        public static implicit operator EntityMetadataFloat(float value) => new EntityMetadataFloat(value);

        public EntityMetadataFloat() { }
        public EntityMetadataFloat(float value) { Value = value; }

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