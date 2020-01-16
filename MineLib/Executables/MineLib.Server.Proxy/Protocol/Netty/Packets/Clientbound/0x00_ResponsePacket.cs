using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets.ClientBound
{
    [Packet(0x00)]
    internal sealed class ResponsePacket : ProxyNettyPacket
    {
		public String JSONResponse = default!;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
			JSONResponse = deserializer.Read(JSONResponse);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(JSONResponse);
        }

    }
}