using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ChangeGameStatePacket : ClientPlayPacket
    {
		public Byte Reason;
		public Single Value;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Reason = deserialiser.Read(Reason);
			Value = deserialiser.Read(Value);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Reason);
            serializer.Write(Value);
        }
    }
}