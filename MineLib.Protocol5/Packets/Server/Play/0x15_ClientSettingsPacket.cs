using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class ClientSettingsPacket : ServerPlayPacket
    {
		public String Locale;
		public SByte ViewDistance;
		public SByte ChatFlags;
		public Boolean ChatColours;
		public SByte Difficulty;
		public Boolean ShowCape;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Locale = deserialiser.Read(Locale);
			ViewDistance = deserialiser.Read(ViewDistance);
			ChatFlags = deserialiser.Read(ChatFlags);
			ChatColours = deserialiser.Read(ChatColours);
			Difficulty = deserialiser.Read(Difficulty);
			ShowCape = deserialiser.Read(ShowCape);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Locale);
            serializer.Write(ViewDistance);
            serializer.Write(ChatFlags);
            serializer.Write(ChatColours);
            serializer.Write(Difficulty);
            serializer.Write(ShowCape);
        }
    }
}