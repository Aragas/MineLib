using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Login
{
    public class LoginStartPacket : ProtobufPacket
    {
		public String Name { get; set; }


        public override VarInt ID => ServerLoginPacketTypes.LoginStart;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Name = reader.Read(Name);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Name);
          
            return this;
        }

    }
}