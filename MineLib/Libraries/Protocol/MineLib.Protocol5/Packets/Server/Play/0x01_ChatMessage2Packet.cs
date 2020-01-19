using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class ChatMessage2Packet : ServerPlayPacket
    {
		public String Message;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Message = deserializer.Read(Message);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Message);
        }
    }
}