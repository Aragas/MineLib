using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Login
{
    public class EncryptionResponsePacket : ProtobufPacket
    {
		public Byte[] SharedSecret;
		public Byte[] VerifyToken;

        public override VarInt ID => ServerLoginPacketTypes.EncryptionResponse;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			var SharedSecretLength = reader.Read<Int16>();
			SharedSecret = reader.Read(SharedSecret, SharedSecretLength);
			var VerifyTokenLength = reader.Read<Int16>();
			VerifyToken = reader.Read(VerifyToken, VerifyTokenLength);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write((Int16) SharedSecret.Length);
			stream.Write(SharedSecret, false);
			stream.Write((Int16) VerifyToken.Length);
			stream.Write(VerifyToken, false);
          
            return this;
        }

    }
}