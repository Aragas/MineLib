using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class PlayerPositionAndLookPacket : ClientPlayPacket
    {
		public Double X;
		public Double Y;
		public Double Z;
		public Single Yaw;
		public Single Pitch;
		public Boolean OnGround;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Yaw = deserialiser.Read(Yaw);
			Pitch = deserialiser.Read(Pitch);
			OnGround = deserialiser.Read(OnGround);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
            serializer.Write(OnGround);
        }
    }
}