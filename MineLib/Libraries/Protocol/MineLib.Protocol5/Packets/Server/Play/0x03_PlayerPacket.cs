using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PlayerPacket : ServerPlayPacket
    {
		public Boolean OnGround;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			OnGround = deserializer.Read(OnGround);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(OnGround);
        }
    }
}