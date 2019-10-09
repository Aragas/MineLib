using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class WindowPropertyPacket : ClientPlayPacket
    {
		public Byte WindowID;
		public Int16 Property;
		public Int16 Value;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			WindowID = deserialiser.Read(WindowID);
			Property = deserialiser.Read(Property);
			Value = deserialiser.Read(Value);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(Property);
            serializer.Write(Value);
        }
    }
}