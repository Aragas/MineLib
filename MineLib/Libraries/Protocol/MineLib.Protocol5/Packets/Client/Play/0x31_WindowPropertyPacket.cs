using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class WindowPropertyPacket : ClientPlayPacket
    {
		public Byte WindowID;
		public Int16 Property;
		public Int16 Value;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			WindowID = deserializer.Read(WindowID);
			Property = deserializer.Read(Property);
			Value = deserializer.Read(Value);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(Property);
            serializer.Write(Value);
        }
    }
}