using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class CloseWindowPacket : ClientPlayPacket
    {
		public Byte WindowID;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			WindowID = deserializer.Read(WindowID);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(WindowID);
        }
    }
}