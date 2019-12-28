using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityEffectPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public SByte EffectID;
		public SByte Amplifier;
		public Int16 Duration;

        public override VarInt ID => ClientPlayPacketTypes.EntityEffect;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			EffectID = reader.Read(EffectID);
			Amplifier = reader.Read(Amplifier);
			Duration = reader.Read(Duration);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(EffectID);
			stream.Write(Amplifier);
			stream.Write(Duration);
          
            return this;
        }

    }
}