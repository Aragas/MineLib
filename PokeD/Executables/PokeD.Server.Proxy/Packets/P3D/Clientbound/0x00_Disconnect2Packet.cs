using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System;

namespace PokeD.Server.Proxy.Packets.P3D.Clientbound
{
    [Packet(0x00)]
    internal sealed class Disconnect2Packet : ProxyP3DPacket
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