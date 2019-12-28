using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Status
{
    public class PingPacket : ClientStatusPacket
    {
		public Int64 Time;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Time = deserialiser.Read(Time);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Time);
        }
    }
}