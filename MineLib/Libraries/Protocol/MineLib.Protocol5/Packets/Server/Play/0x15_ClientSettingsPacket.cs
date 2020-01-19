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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Locale = deserializer.Read(Locale);
			ViewDistance = deserializer.Read(ViewDistance);
			ChatFlags = deserializer.Read(ChatFlags);
			ChatColours = deserializer.Read(ChatColours);
			Difficulty = deserializer.Read(Difficulty);
			ShowCape = deserializer.Read(ShowCape);
        }

        public override void Serialize(IPacketSerializer serializer)
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