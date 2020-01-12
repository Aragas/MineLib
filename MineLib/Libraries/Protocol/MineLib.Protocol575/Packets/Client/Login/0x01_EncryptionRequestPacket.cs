using System;

using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Login
{
    public class EncryptionRequestPacket : ClientLoginPacket
    {
		public String ServerID;
		public Byte[] PublicKey;
		public Byte[] VerifyToken;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			ServerID = deserialiser.Read(ServerID);
			var PublicKeyLength = deserialiser.Read<Int16>();
			PublicKey = deserialiser.Read(PublicKey, PublicKeyLength);
			var VerifyTokenLength = deserialiser.Read<Int16>();
			VerifyToken = deserialiser.Read(VerifyToken, VerifyTokenLength);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(ServerID);
            serializer.Write((Int16) PublicKey.Length);
            serializer.Write(PublicKey, false);
            serializer.Write((Int16) VerifyToken.Length);
            serializer.Write(VerifyToken, false);
        }
    }
}