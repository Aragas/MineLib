using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class JoinGamePacket : ProtobufPacket
    {
		public Int32 EntityID;
		public Byte GameMode;
		public SByte Dimension;
		public Byte Difficulty;
		public Byte MaxPlayers;
		public String LevelType;

        public override VarInt ID => ClientPlayPacketTypes.JoinGame;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			GameMode = reader.Read(GameMode);
			Dimension = reader.Read(Dimension);
			Difficulty = reader.Read(Difficulty);
			MaxPlayers = reader.Read(MaxPlayers);
			LevelType = reader.Read(LevelType);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(GameMode);
			stream.Write(Dimension);
			stream.Write(Difficulty);
			stream.Write(MaxPlayers);
			stream.Write(LevelType);
          
            return this;
        }

    }
}