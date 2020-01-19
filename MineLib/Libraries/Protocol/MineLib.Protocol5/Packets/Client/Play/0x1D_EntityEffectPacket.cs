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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			EffectID = deserialiser.Read(EffectID);
			Amplifier = deserialiser.Read(Amplifier);
			Duration = deserialiser.Read(Duration);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(EffectID);
            serializer.Write(Amplifier);
            serializer.Write(Duration);
        }
    }
}