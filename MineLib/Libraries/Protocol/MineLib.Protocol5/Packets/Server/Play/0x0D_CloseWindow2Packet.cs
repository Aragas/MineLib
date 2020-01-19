using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class CloseWindow2Packet : ServerPlayPacket
    {
		public SByte WindowID;

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