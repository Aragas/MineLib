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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Radius = deserializer.Read(Radius);
			var RecordsLength = deserializer.Read<Int32>();
			Records = deserializer.Read(Records, RecordsLength / 3);
			PlayerMotionX = deserializer.Read(PlayerMotionX);
			PlayerMotionY = deserializer.Read(PlayerMotionY);
			PlayerMotionZ = deserializer.Read(PlayerMotionZ);
        }

        public override void Serialize(IPacketSerializer serializer)
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