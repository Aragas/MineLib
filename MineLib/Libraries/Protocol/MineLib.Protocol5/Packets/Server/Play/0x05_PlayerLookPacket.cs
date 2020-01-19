using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PlayerLookPacket : ServerPlayPacket
    {
		public Single Yaw;
		public Single Pitch;
		public Boolean OnGround;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Yaw = deserialiser.Read(Yaw);
			Pitch = deserialiser.Read(Pitch);
			OnGround = deserialiser.Read(OnGround);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Yaw);
            serializer.Write(Pitch);
            serializer.Write(OnGround);
        }
    }
}