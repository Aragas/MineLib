using System;

using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Server.Login
{
    public class LoginStartPacket : ServerLoginPacket
    {
		public String Name { get; set; }

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Name = deserialiser.Read(Name);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Name);
        }
    }
}