using System;

using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Data.EntityMetadata
{
    /// <summary>
    /// String Metadata
    /// </summary>
    public class EntityMetadataString : EntityMetadataEntry
    {
        protected override byte Identifier => 4;
        protected override string FriendlyName => "string";

        public string Value;

        public static implicit operator EntityMetadataString(string value) => new EntityMetadataString(value);

        public EntityMetadataString() { }
        public EntityMetadataString(string value)
        {
            if (value.Length > 16)
                throw new ArgumentOutOfRangeException(nameof(value), "Maximum string length is 16 characters");
            while (value.Length < 16)
                value += "\0";
            Value = value;
        }

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