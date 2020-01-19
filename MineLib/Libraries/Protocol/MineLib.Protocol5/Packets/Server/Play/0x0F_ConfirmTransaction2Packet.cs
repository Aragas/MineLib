using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class ConfirmTransaction2Packet : ServerPlayPacket
    {
		public SByte WindowID;
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