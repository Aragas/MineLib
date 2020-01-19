using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class TabCompletePacket : ClientPlayPacket
    {
		public String[] Match;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Match = deserializer.Read(Match);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Match);
        }
    }
}