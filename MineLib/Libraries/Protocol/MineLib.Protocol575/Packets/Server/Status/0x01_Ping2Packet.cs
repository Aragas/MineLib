using System;
using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Server.Status
{
    [PacketID(0x01)]
    public class Ping2Packet : ServerStatusPacket
    {
		public Int64 Time;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Time = deserializer.Read(Time);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Time);
        }
    }
}