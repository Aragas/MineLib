using System;

using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Proxy.Packets.Netty.Serverbound
{
    [Packet(0x01)]
    public sealed class PingPacket : StatusStatePacket
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