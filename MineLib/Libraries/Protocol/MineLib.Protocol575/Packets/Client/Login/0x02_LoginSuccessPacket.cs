using System;

using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Login
{
    public class LoginSuccessPacket : ClientLoginPacket
    {
		public String UUID;
		public String Username;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			UUID = deserialiser.Read(UUID);
			Username = deserialiser.Read(Username);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(UUID);
            serializer.Write(Username);
        }
    }
}