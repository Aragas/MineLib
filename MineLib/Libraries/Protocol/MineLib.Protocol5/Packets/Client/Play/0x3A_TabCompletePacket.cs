using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class TabCompletePacket : ClientPlayPacket
    {
		public String[] Match;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Match = deserialiser.Read(Match);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Match);
        }
    }
}