using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PlayerLookPacket : ServerPlayPacket
    {
		public Single Yaw;
		public Single Pitch;
		public Boolean OnGround;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Yaw = deserializer.Read(Yaw);
			Pitch = deserializer.Read(Pitch);
			OnGround = deserializer.Read(OnGround);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Yaw);
            serializer.Write(Pitch);
            serializer.Write(OnGround);
        }
    }
}