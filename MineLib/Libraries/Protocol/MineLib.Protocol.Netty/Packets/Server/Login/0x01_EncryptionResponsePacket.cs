using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System;

namespace MineLib.Protocol.Netty.Packets.Server.Login
{
    [Packet(0x01)]
    public class EncryptionResponsePacket : ServerLoginPacket
    {
		public Byte[] SharedSecret;
		public Byte[] VerifyToken;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			var SharedSecretLength = deserializer.Read<Int16>();
			SharedSecret = deserializer.Read(SharedSecret, SharedSecretLength);
			var VerifyTokenLength = deserializer.Read<Int16>();
			VerifyToken = deserializer.Read(VerifyToken, VerifyTokenLength);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write((Int16) SharedSecret.Length);
            serializer.Write(SharedSecret, false);
            serializer.Write((Int16) VerifyToken.Length);
            serializer.Write(VerifyToken, false);
        }

    }
}