using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class TabComplete2Packet : ServerPlayPacket
    {
		public String Text;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Text = deserializer.Read(Text);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Text);
        }
    }
}