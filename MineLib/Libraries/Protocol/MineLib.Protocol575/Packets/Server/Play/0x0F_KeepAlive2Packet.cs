using System;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Server.Play
{
    public class KeepAlive2Packet : ServerPlayPacket
    {
		public Int64 KeepAliveID;

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