using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SoundEffectPacket : ClientPlayPacket
    {
		public String SoundName;
		public Int32 EffectPositionX;
		public Int32 EffectPositionY;
		public Int32 EffectPositionZ;
		public Single Volume;
		public Byte Pitch;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			SoundName = deserialiser.Read(SoundName);
			EffectPositionX = deserialiser.Read(EffectPositionX);
			EffectPositionY = deserialiser.Read(EffectPositionY);
			EffectPositionZ = deserialiser.Read(EffectPositionZ);
			Volume = deserialiser.Read(Volume);
			Pitch = deserialiser.Read(Pitch);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(SoundName);
            serializer.Write(EffectPositionX);
            serializer.Write(EffectPositionY);
            serializer.Write(EffectPositionZ);
            serializer.Write(Volume);
            serializer.Write(Pitch);
        }
    }
}