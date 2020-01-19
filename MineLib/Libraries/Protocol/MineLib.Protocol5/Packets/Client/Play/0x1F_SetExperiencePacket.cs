using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SetExperiencePacket : ClientPlayPacket
    {
		public Single ExperienceBar;
		public Int16 Level;
		public Int16 TotalExperience;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			ExperienceBar = deserializer.Read(ExperienceBar);
			Level = deserializer.Read(Level);
			TotalExperience = deserializer.Read(TotalExperience);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ExperienceBar);
            serializer.Write(Level);
            serializer.Write(TotalExperience);
        }
    }
}