using System;

using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Server.Status
{
    public class Ping2Packet : ServerStatusPacket
    {
		public Int64 Time;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Time = deserialiser.Read(Time);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Time);
        }
    }
}