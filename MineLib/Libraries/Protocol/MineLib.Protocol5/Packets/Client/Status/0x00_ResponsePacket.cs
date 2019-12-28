using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Status
{
    public class ResponsePacket : ClientStatusPacket
    {
		public String JSONResponse;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			JSONResponse = deserialiser.Read(JSONResponse);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(JSONResponse);
        }
    }
}