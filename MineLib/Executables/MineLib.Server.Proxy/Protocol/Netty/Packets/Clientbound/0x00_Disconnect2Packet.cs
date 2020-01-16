using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets.ClientBound
{
    [Packet(0x00)]
    internal sealed class Disconnect2Packet : ProxyNettyPacket
    {
		public String JSONData = default!;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
			JSONData = deserializer.Read(JSONData);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(JSONData);
        }
    }
}