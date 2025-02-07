using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Status
{
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