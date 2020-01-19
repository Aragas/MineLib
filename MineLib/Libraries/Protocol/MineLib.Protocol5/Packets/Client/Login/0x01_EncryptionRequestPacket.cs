using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Login
{
    public class EncryptionRequestPacket : ClientLoginPacket
    {
		public String ServerID;
		public Byte[] PublicKey;
		public Byte[] VerifyToken;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			ServerID = deserializer.Read(ServerID);
			var PublicKeyLength = deserializer.Read<Int16>();
			PublicKey = deserializer.Read(PublicKey, PublicKeyLength);
			var VerifyTokenLength = deserializer.Read<Int16>();
			VerifyToken = deserializer.Read(VerifyToken, VerifyTokenLength);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ServerID);
            serializer.Write((Int16) PublicKey.Length);
            serializer.Write(PublicKey, false);
            serializer.Write((Int16) VerifyToken.Length);
            serializer.Write(VerifyToken, false);
        }
    }
}