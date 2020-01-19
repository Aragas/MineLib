using Aragas.Network.IO;

using System;

namespace MineLib.Protocol.Netty.Packets.Client.Status
{
    public class ResponsePacket : ClientStatusPacket
    {
		public String JSONResponse;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			JSONResponse = deserialiser.Read(JSONResponse);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(JSONResponse);
        }
    }
}