using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityEffectPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte EffectID;
		public SByte Amplifier;
		public Int16 Duration;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			EffectID = deserialiser.Read(EffectID);
			Amplifier = deserialiser.Read(Amplifier);
			Duration = deserialiser.Read(Duration);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(EffectID);
            serializer.Write(Amplifier);
            serializer.Write(Duration);
        }
    }
}