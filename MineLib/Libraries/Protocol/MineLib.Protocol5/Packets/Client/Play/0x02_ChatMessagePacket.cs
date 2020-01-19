using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ChatMessagePacket : ClientPlayPacket
    {
		public String JSONData;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			JSONData = deserialiser.Read(JSONData);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(JSONData);
        }
    }
}