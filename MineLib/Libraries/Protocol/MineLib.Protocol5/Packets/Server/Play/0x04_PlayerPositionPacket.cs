using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PlayerPositionPacket : ServerPlayPacket
    {
		public Double X;
		public Double FeetY;
		public Double HeadY;
		public Double Z;
		public Boolean OnGround;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			X = deserializer.Read(X);
			FeetY = deserializer.Read(FeetY);
			HeadY = deserializer.Read(HeadY);
			Z = deserializer.Read(Z);
			OnGround = deserializer.Read(OnGround);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(FeetY);
            serializer.Write(HeadY);
            serializer.Write(Z);
            serializer.Write(OnGround);
        }
    }
}