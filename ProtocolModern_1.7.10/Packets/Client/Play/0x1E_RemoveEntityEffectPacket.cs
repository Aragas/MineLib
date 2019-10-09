using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class RemoveEntityEffectPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public SByte EffectID;

        public override VarInt ID => ClientPlayPacketTypes.RemoveEntityEffect;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			EffectID = reader.Read(EffectID);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(EffectID);
          
            return this;
        }

    }
}