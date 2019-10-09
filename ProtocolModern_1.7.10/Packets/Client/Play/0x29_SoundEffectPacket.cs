using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class SoundEffectPacket : ProtobufPacket
    {
		public String SoundName;
		public Int32 EffectPositionX;
		public Int32 EffectPositionY;
		public Int32 EffectPositionZ;
		public Single Volume;
		public Byte Pitch;

        public override VarInt ID => ClientPlayPacketTypes.SoundEffect;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			SoundName = reader.Read(SoundName);
			EffectPositionX = reader.Read(EffectPositionX);
			EffectPositionY = reader.Read(EffectPositionY);
			EffectPositionZ = reader.Read(EffectPositionZ);
			Volume = reader.Read(Volume);
			Pitch = reader.Read(Pitch);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(SoundName);
			stream.Write(EffectPositionX);
			stream.Write(EffectPositionY);
			stream.Write(EffectPositionZ);
			stream.Write(Volume);
			stream.Write(Pitch);
          
            return this;
        }

    }
}