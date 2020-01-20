using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Data.EntityMetadata
{
    /// <summary>
    /// Rotation Metadata
    /// </summary>
    public class EntityMetadataRotation : EntityMetadataEntry
    {
        protected override byte Identifier => 7;
        protected override string FriendlyName => "rotation";

        public Rotation Value;

        public static implicit operator EntityMetadataRotation(in Rotation value) => new EntityMetadataRotation(value);

        public EntityMetadataRotation() { Value = new Rotation(0,0,0); }
        public EntityMetadataRotation(float pitch, float yaw, float roll) { Value = new Rotation(pitch, yaw, roll); }
        public EntityMetadataRotation(in Rotation rotation) { Value = rotation; }

        public override void FromDeserializer(IPacketDeserializer deserializer)
        {
            Value = new Rotation(
                deserializer.Read<float>(),
                deserializer.Read<float>(),
                deserializer.Read<float>());
        }
        public override void ToSerializer(IPacketSerializer serializer, byte index)
        {
            serializer.Write(GetKey(index));
            serializer.Write(Value.Pitch);
            serializer.Write(Value.Yaw);
            serializer.Write(Value.Roll);
        }
    }
}