using System;

using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Proxy.Packets.Netty.Clientbound
{
    [Packet(0x00)]
    internal sealed class Disconnect2Packet : ProxyNettyPacket
    {
		public String JSONData;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			JSONData = deserialiser.Read(JSONData);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(JSONData);
        }
    }
}