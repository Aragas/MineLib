using System;

using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Status
{
    [PacketID(0x00)]
    public class PingPacket : ClientStatusPacket
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