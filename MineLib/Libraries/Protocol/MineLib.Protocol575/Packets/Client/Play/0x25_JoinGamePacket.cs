using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Play
{
    public class JoinGamePacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Byte GameMode;
		public Int32 Dimension;
        public Byte MaxPlayers;
        public String LevelType;
        public VarInt ViewDistance;
        public Boolean ReducedDebugInfo;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			GameMode = deserialiser.Read(GameMode);
			Dimension = deserialiser.Read(Dimension);
            MaxPlayers = deserialiser.Read(MaxPlayers);
            LevelType = deserialiser.Read(LevelType);
            ViewDistance = deserialiser.Read(ViewDistance);
            ReducedDebugInfo = deserialiser.Read(ReducedDebugInfo);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(GameMode);
            serializer.Write(Dimension);
            serializer.Write(MaxPlayers);
            serializer.Write(LevelType);
            serializer.Write(ViewDistance);
            serializer.Write(ReducedDebugInfo);
        }
    }
}