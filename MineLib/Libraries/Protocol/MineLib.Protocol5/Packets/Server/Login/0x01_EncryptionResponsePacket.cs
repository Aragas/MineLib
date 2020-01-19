using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Login
{
    public class EncryptionResponsePacket : ServerLoginPacket
    {
		public Byte[] SharedSecret;
		public Byte[] VerifyToken;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			var SharedSecretLength = deserialiser.Read<Int16>();
			SharedSecret = deserialiser.Read(SharedSecret, SharedSecretLength);
			var VerifyTokenLength = deserialiser.Read<Int16>();
			VerifyToken = deserialiser.Read(VerifyToken, VerifyTokenLength);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write((Int16) SharedSecret.Length);
            serializer.Write(SharedSecret, false);
            serializer.Write((Int16) VerifyToken.Length);
            serializer.Write(VerifyToken, false);
        }

    }
}