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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			GameMode = deserialiser.Read(GameMode);
			Dimension = deserialiser.Read(Dimension);
			Difficulty = deserialiser.Read(Difficulty);
			MaxPlayers = deserialiser.Read(MaxPlayers);
			LevelType = deserialiser.Read(LevelType);
        }

        public override void Serialize(IStreamSerializer serializer)
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