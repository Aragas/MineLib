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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			X = deserialiser.Read(X);
			FeetY = deserialiser.Read(FeetY);
			HeadY = deserialiser.Read(HeadY);
			Z = deserialiser.Read(Z);
			Yaw = deserialiser.Read(Yaw);
			Pitch = deserialiser.Read(Pitch);
			OnGround = deserialiser.Read(OnGround);
        }

        public override void Serialize(IStreamSerializer serializer)
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