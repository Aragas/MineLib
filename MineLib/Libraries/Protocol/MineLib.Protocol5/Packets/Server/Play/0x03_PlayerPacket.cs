using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PlayerPacket : ServerPlayPacket
    {
		public Boolean OnGround;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			OnGround = deserialiser.Read(OnGround);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(OnGround);
        }
    }
}