using System;

using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Server.Login
{
    public class LoginStartPacket : ServerLoginPacket
    {
		public String Name { get; set; }

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Name = deserializer.Read(Name);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Name);
        }
    }
}