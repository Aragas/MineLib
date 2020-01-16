using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets.ClientBound
{
    [Packet(0x01)]
    internal sealed class PongPacket : ProxyNettyPacket
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