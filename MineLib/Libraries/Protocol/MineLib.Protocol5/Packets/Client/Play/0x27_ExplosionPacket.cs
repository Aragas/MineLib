using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ExplosionPacket : ClientPlayPacket
    {
		public Single X;
		public Single Y;
		public Single Z;
		public Single Radius;
		public Byte[] Records;
		public Single PlayerMotionX;
		public Single PlayerMotionY;
		public Single PlayerMotionZ;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Radius = deserialiser.Read(Radius);
			var RecordsLength = deserialiser.Read<Int32>();
			Records = deserialiser.Read(Records, RecordsLength / 3);
			PlayerMotionX = deserialiser.Read(PlayerMotionX);
			PlayerMotionY = deserialiser.Read(PlayerMotionY);
			PlayerMotionZ = deserialiser.Read(PlayerMotionZ);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Radius);
            serializer.Write(Records.Length * 3);
            serializer.Write(Records, false);
            serializer.Write(PlayerMotionX);
            serializer.Write(PlayerMotionY);
            serializer.Write(PlayerMotionZ);
        }
    }
}