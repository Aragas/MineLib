using System;

using Aragas.Core.IO;

using MineLib.Core.Data;

namespace ProtocolModern.Data.EntityMetadata
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
                value = value + "\0";
            Value = value;
        }

        public override void FromReader(PacketDataReader reader)
        {
            Value = reader.Read(Value);
        }
        public override void ToStream(PacketStream stream, byte index)
        {
            stream.Write(GetKey(index));
            stream.Write(Value);
        }
    }
}