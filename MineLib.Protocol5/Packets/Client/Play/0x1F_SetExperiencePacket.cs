using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SetExperiencePacket : ClientPlayPacket
    {
		public Single ExperienceBar;
		public Int16 Level;
		public Int16 TotalExperience;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			ExperienceBar = deserialiser.Read(ExperienceBar);
			Level = deserialiser.Read(Level);
			TotalExperience = deserialiser.Read(TotalExperience);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(ExperienceBar);
            serializer.Write(Level);
            serializer.Write(TotalExperience);
        }
    }
}