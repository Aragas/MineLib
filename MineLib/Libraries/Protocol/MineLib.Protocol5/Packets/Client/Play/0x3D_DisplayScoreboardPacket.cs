using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class DisplayScoreboardPacket : ClientPlayPacket
    {
		public SByte Position;
		public String ScoreName;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Position = deserialiser.Read(Position);
			ScoreName = deserialiser.Read(ScoreName);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Position);
            serializer.Write(ScoreName);
        }
    }
}