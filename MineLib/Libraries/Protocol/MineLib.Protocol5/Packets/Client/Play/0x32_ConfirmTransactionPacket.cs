using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ConfirmTransactionPacket : ClientPlayPacket
    {
		public Byte WindowID;
		public Int16 ActionNumber;
		public Boolean Accepted;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			WindowID = deserializer.Read(WindowID);
			ActionNumber = deserializer.Read(ActionNumber);
			Accepted = deserializer.Read(Accepted);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(ActionNumber);
            serializer.Write(Accepted);
        }
    }
}