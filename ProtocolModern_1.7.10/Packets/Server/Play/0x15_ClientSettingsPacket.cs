using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class ClientSettingsPacket : ProtobufPacket
    {
		public String Locale;
		public SByte ViewDistance;
		public SByte ChatFlags;
		public Boolean ChatColours;
		public SByte Difficulty;
		public Boolean ShowCape;

        public override VarInt ID => ServerPlayPacketTypes.ClientSettings;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Locale = reader.Read(Locale);
			ViewDistance = reader.Read(ViewDistance);
			ChatFlags = reader.Read(ChatFlags);
			ChatColours = reader.Read(ChatColours);
			Difficulty = reader.Read(Difficulty);
			ShowCape = reader.Read(ShowCape);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Locale);
			stream.Write(ViewDistance);
			stream.Write(ChatFlags);
			stream.Write(ChatColours);
			stream.Write(Difficulty);
			stream.Write(ShowCape);
          
            return this;
        }

    }
}