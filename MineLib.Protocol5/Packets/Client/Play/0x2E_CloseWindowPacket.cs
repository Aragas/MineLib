using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class CloseWindowPacket : ClientPlayPacket
    {
		public Byte WindowID;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			WindowID = deserialiser.Read(WindowID);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(WindowID);
        }
    }
}