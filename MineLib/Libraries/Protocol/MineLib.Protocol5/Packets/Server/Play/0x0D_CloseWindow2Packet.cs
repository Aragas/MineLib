using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class CloseWindow2Packet : ServerPlayPacket
    {
		public SByte WindowID;

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