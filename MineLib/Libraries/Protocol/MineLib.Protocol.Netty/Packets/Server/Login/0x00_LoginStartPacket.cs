using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System;

namespace MineLib.Protocol.Netty.Packets.Server.Login
{
    [Packet(0x00)]
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