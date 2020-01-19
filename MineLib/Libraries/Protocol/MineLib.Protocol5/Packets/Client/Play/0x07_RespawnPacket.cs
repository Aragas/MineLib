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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Dimension = deserializer.Read(Dimension);
			Difficulty = deserializer.Read(Difficulty);
			GameMode = deserializer.Read(GameMode);
			LevelType = deserializer.Read(LevelType);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Dimension);
            serializer.Write(Difficulty);
            serializer.Write(GameMode);
            serializer.Write(LevelType);
        }
    }
}