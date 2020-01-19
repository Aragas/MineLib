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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			GameMode = deserializer.Read(GameMode);
			Dimension = deserializer.Read(Dimension);
            MaxPlayers = deserializer.Read(MaxPlayers);
            LevelType = deserializer.Read(LevelType);
            ViewDistance = deserializer.Read(ViewDistance);
            ReducedDebugInfo = deserializer.Read(ReducedDebugInfo);
        }

        public override void Serialize(IPacketSerializer serializer)
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