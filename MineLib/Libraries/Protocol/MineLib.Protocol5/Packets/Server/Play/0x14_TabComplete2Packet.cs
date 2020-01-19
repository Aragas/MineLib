using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class TabComplete2Packet : ServerPlayPacket
    {
		public String Text;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Text = deserialiser.Read(Text);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Text);
        }
    }
}