using System;
using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Server.Play
{
    [PacketID(0x0F)]
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