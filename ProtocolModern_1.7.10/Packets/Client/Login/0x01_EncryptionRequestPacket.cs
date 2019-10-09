using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Login
{
    public class EncryptionRequestPacket : ProtobufPacket
    {
		public String ServerID;
		public Byte[] PublicKey;
		public Byte[] VerifyToken;

        public override VarInt ID => ClientLoginPacketTypes.EncryptionRequest;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			ServerID = reader.Read(ServerID);
			var PublicKeyLength = reader.Read<Int16>();
			PublicKey = reader.Read(PublicKey, PublicKeyLength);
			var VerifyTokenLength = reader.Read<Int16>();
			VerifyToken = reader.Read(VerifyToken, VerifyTokenLength);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(ServerID);
			stream.Write((Int16) PublicKey.Length);
			stream.Write(PublicKey, false);
			stream.Write((Int16) VerifyToken.Length);
			stream.Write(VerifyToken, false);
          
            return this;
        }

    }
}