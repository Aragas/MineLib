using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class TabComplete2Packet : ServerPlayPacket
    {
		public String Text;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Text = deserialiser.Read(Text);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Text);
        }
    }
}