using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class DisplayScoreboardPacket : ClientPlayPacket
    {
		public SByte Position;
		public String ScoreName;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Position = deserializer.Read(Position);
			ScoreName = deserializer.Read(ScoreName);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Position);
            serializer.Write(ScoreName);
        }
    }
}