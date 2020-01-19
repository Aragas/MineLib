using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SignEditorOpenPacket : ClientPlayPacket
    {
		public Int32 X;
		public Int32 Y;
		public Int32 Z;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
        }
    }
}