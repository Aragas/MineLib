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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			EffectID = deserializer.Read(EffectID);
			Amplifier = deserializer.Read(Amplifier);
			Duration = deserializer.Read(Duration);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(EffectID);
            serializer.Write(Amplifier);
            serializer.Write(Duration);
        }
    }
}