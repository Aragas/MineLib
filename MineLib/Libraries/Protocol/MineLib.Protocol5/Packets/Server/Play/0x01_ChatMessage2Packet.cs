using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class ChatMessage2Packet : ServerPlayPacket
    {
		public String Message;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Message = deserialiser.Read(Message);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Message);
        }
    }
}