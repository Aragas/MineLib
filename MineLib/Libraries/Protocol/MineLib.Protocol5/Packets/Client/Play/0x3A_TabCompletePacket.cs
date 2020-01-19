using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class TabCompletePacket : ClientPlayPacket
    {
		public String[] Match;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Match = deserialiser.Read(Match);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Match);
        }
    }
}