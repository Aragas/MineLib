using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class JoinGamePacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Byte GameMode;
		public SByte Dimension;
		public Byte Difficulty;
		public Byte MaxPlayers;
		public String LevelType;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			GameMode = deserializer.Read(GameMode);
			Dimension = deserializer.Read(Dimension);
			Difficulty = deserializer.Read(Difficulty);
			MaxPlayers = deserializer.Read(MaxPlayers);
			LevelType = deserializer.Read(LevelType);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(GameMode);
            serializer.Write(Dimension);
            serializer.Write(Difficulty);
            serializer.Write(MaxPlayers);
            serializer.Write(LevelType);
        }
    }
}