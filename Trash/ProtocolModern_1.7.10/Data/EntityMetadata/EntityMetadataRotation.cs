using Aragas.Core.IO;

using MineLib.Core.Data;

namespace ProtocolModern.Data.EntityMetadata
{
    /// <summary>
    /// Rotation Metadata
    /// </summary>
    public class EntityMetadataRotation : EntityMetadataEntry
    {
        protected override byte Identifier => 7;
        protected override string FriendlyName => "rotation";

        public Rotation Value;

        public static implicit operator EntityMetadataRotation(Rotation value) => new EntityMetadataRotation(value);

        public EntityMetadataRotation() { Value = new Rotation(0,0,0); }
        public EntityMetadataRotation(float pitch, float yaw, float roll) { Value = new Rotation(pitch, yaw, roll); }
        public EntityMetadataRotation(Rotation rotation) { Value = rotation; }

        public override void FromReader(PacketDataReader reader)
        {
            Value = new Rotation(
                reader.Read<float>(),
                reader.Read<float>(),
                reader.Read<float>());
        }
        public override void ToStream(PacketStream stream, byte index)
        {
            stream.Write(GetKey(index));

            stream.Write(Value.Pitch);
            stream.Write(Value.Yaw);
            stream.Write(Value.Roll);
        }
    }
}
