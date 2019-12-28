using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Login
{
    public class LoginSuccessPacket : ProtobufPacket
    {
		public String UUID;
		public String Username;

        public override VarInt ID => ClientLoginPacketTypes.LoginSuccess;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			UUID = reader.Read(UUID);
			Username = reader.Read(Username);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(UUID);
			stream.Write(Username);
          
            return this;
        }

    }
}