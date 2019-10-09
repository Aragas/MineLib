using System;
using System.Collections.Generic;
using System.Text;

using Aragas.Core;
using Aragas.Core.Packets;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

using ProtocolModern.Packets.Client.Login;
using ProtocolModern.Packets.Server.Login;

namespace ProtocolModern.PacketHandlers.Server.Login
{
    public class EncryptionRequestHandler : ProtocolPacketHandler<EncryptionRequestPacket>
    {
        public override ProtobufPacket Handle(EncryptionRequestPacket packet)
        {
            var generator = new CipherKeyGenerator();
            generator.Init(new KeyGenerationParameters(new SecureRandom(), 16 * 8));
            var sharedKey = generator.GenerateKey();

            var hash = GetServerIDHash(packet.PublicKey, sharedKey, packet.ServerID);
            if (!Yggdrasil.JoinSession(Context.AccessToken, Context.SelectedProfile, hash).Result.Response)
                throw new Exception("Yggdrasil error: Not authenticated.");

            var signer = new PKCS1Signer(packet.PublicKey);
            Context.SendPacket(new EncryptionResponsePacket
            {
                SharedSecret = signer.SignData(sharedKey),
                VerifyToken = signer.SignData(packet.VerifyToken)
            });

            Context.Stream.InitializeEncryption(sharedKey);

            return null;
        }

        private static string GetServerIDHash(byte[] publicKey, byte[] secretKey, string serverID)
        {
            var hashlist = new List<byte>();
            hashlist.AddRange(Encoding.UTF8.GetBytes(serverID));
            hashlist.AddRange(secretKey);
            hashlist.AddRange(publicKey);

            return JavaHelper.JavaHexDigest(hashlist.ToArray());
        }
    }
}
