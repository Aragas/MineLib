using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Login
{
    public class LoginSuccessPacket : ClientLoginPacket
    {
		public String UUID;
		public String Username;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			UUID = deserializer.Read(UUID);
			Username = deserializer.Read(Username);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(UUID);
            serializer.Write(Username);
        }
    }
}