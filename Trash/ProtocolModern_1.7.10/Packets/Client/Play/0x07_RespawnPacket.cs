using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class RespawnPacket : ProtobufPacket
    {
		public Int32 Dimension;
		public Byte Difficulty;
		public Byte GameMode;
		public String LevelType;

        public override VarInt ID => ClientPlayPacketTypes.Respawn;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Dimension = reader.Read(Dimension);
			Difficulty = reader.Read(Difficulty);
			GameMode = reader.Read(GameMode);
			LevelType = reader.Read(LevelType);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Dimension);
			stream.Write(Difficulty);
			stream.Write(GameMode);
			stream.Write(LevelType);
          
            return this;
        }

    }
}