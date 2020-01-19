using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PlayerPositionAndLook2Packet : ServerPlayPacket
    {
		public Double X;
		public Double FeetY;
		public Double HeadY;
		public Double Z;
		public Single Yaw;
		public Single Pitch;
		public Boolean OnGround;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			X = deserializer.Read(X);
			FeetY = deserializer.Read(FeetY);
			HeadY = deserializer.Read(HeadY);
			Z = deserializer.Read(Z);
			Yaw = deserializer.Read(Yaw);
			Pitch = deserializer.Read(Pitch);
			OnGround = deserializer.Read(OnGround);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(FeetY);
            serializer.Write(HeadY);
            serializer.Write(Z);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
            serializer.Write(OnGround);
        }
    }
}