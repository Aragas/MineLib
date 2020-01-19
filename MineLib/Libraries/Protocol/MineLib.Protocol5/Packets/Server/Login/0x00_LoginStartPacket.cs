using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Login
{
    public class LoginStartPacket : ServerLoginPacket
    {
		public String Name { get; set; }

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Name = deserialiser.Read(Name);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Name);
        }
    }
}