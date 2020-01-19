using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ChatMessagePacket : ClientPlayPacket
    {
		public String JSONData;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			JSONData = deserializer.Read(JSONData);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(JSONData);
        }
    }
}