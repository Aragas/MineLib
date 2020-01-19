using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System;

namespace MineLib.Protocol.Netty.Packets.Client.Status
{
    [Packet(0x00)]
    public class ResponsePacket : ClientStatusPacket
    {
		public String JSONResponse;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			JSONResponse = deserializer.Read(JSONResponse);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(JSONResponse);
        }
    }
}