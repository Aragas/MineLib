using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class DisconnectPacket : ClientPlayPacket
    {
		public String Reason;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Reason = deserialiser.Read(Reason);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Reason);
        }
    }
}