using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ChangeGameStatePacket : ClientPlayPacket
    {
		public Byte Reason;
		public Single Value;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Reason = deserializer.Read(Reason);
			Value = deserializer.Read(Value);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Reason);
            serializer.Write(Value);
        }
    }
}