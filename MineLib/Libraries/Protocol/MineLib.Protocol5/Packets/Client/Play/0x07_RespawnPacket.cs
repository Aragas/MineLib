using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class RespawnPacket : ClientPlayPacket
    {
		public Int32 Dimension;
		public Byte Difficulty;
		public Byte GameMode;
		public String LevelType;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Dimension = deserialiser.Read(Dimension);
			Difficulty = deserialiser.Read(Difficulty);
			GameMode = deserialiser.Read(GameMode);
			LevelType = deserialiser.Read(LevelType);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Dimension);
            serializer.Write(Difficulty);
            serializer.Write(GameMode);
            serializer.Write(LevelType);
        }
    }
}