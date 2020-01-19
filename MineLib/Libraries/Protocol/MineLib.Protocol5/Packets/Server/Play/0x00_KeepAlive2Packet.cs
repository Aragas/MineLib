using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class KeepAlive2Packet : ServerPlayPacket
    {
		public Int32 KeepAliveID;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			KeepAliveID = deserializer.Read(KeepAliveID);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(KeepAliveID);
        }
    }
}