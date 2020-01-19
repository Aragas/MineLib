using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Status
{
    public class PingPacket : ClientStatusPacket
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