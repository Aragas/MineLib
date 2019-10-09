using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class ExplosionPacket : ProtobufPacket
    {
		public Single X;
		public Single Y;
		public Single Z;
		public Single Radius;
		public Byte[] Records;
		public Single PlayerMotionX;
		public Single PlayerMotionY;
		public Single PlayerMotionZ;

        public override VarInt ID => ClientPlayPacketTypes.Explosion;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Radius = reader.Read(Radius);
			var RecordsLength = reader.Read<Int32>();
			Records = reader.Read(Records, RecordsLength / 3);
			PlayerMotionX = reader.Read(PlayerMotionX);
			PlayerMotionY = reader.Read(PlayerMotionY);
			PlayerMotionZ = reader.Read(PlayerMotionZ);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Radius);
			stream.Write(Records.Length * 3);
			stream.Write(Records, false);
			stream.Write(PlayerMotionX);
			stream.Write(PlayerMotionY);
			stream.Write(PlayerMotionZ);
          
            return this;
        }

    }
}