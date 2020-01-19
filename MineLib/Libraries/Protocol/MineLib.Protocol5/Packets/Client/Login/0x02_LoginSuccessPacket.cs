using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Login
{
    public class LoginSuccessPacket : ClientLoginPacket
    {
		public String UUID;
		public String Username;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			UUID = deserialiser.Read(UUID);
			Username = deserialiser.Read(Username);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(UUID);
            serializer.Write(Username);
        }
    }
}