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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			SoundName = deserializer.Read(SoundName);
			EffectPositionX = deserializer.Read(EffectPositionX);
			EffectPositionY = deserializer.Read(EffectPositionY);
			EffectPositionZ = deserializer.Read(EffectPositionZ);
			Volume = deserializer.Read(Volume);
			Pitch = deserializer.Read(Pitch);
        }

        public override void Serialize(IPacketSerializer serializer)
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