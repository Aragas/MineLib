using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class SetExperiencePacket : ProtobufPacket
    {
		public Single ExperienceBar;
		public Int16 Level;
		public Int16 TotalExperience;

        public override VarInt ID => ClientPlayPacketTypes.SetExperience;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			ExperienceBar = reader.Read(ExperienceBar);
			Level = reader.Read(Level);
			TotalExperience = reader.Read(TotalExperience);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(ExperienceBar);
			stream.Write(Level);
			stream.Write(TotalExperience);
          
            return this;
        }

    }
}