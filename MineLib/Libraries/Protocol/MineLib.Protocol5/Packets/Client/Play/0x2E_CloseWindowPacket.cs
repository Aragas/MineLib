using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class CloseWindowPacket : ClientPlayPacket
    {
		public Byte WindowID;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			WindowID = deserialiser.Read(WindowID);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(WindowID);
        }
    }
}