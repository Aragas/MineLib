using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ConfirmTransactionPacket : ClientPlayPacket
    {
		public Byte WindowID;
		public Int16 ActionNumber;
		public Boolean Accepted;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			WindowID = deserialiser.Read(WindowID);
			ActionNumber = deserialiser.Read(ActionNumber);
			Accepted = deserialiser.Read(Accepted);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(ActionNumber);
            serializer.Write(Accepted);
        }
    }
}