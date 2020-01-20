using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Data.EntityMetadata
{
    /// <summary>
    /// Slot Metadata
    /// </summary>
    public class EntityMetadataSlot : EntityMetadataEntry
    {
        protected override byte Identifier => 5;
        protected override string FriendlyName => "slot";

        public ItemSlot Value;

        public static implicit operator EntityMetadataSlot(in ItemSlot value) => new EntityMetadataSlot(value);

        public EntityMetadataSlot() { }
        public EntityMetadataSlot(in ItemSlot value) { Value = value; }

        public override void FromDeserializer(IPacketDeserializer deserializer)
        {
            Value = deserializer.Read<ItemSlot>();
        }
        public override void ToSerializer(IPacketSerializer serializer, byte index)
        {
            serializer.Write(GetKey(index));
            serializer.Write(Value);
        }
    }
}