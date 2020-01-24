using System;

using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Play
{
    [Packet(0x25)]
    public class JoinGamePacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Byte GameMode;
		public Int32 Dimension;
        //public Int64 HashedSeed;
        public Byte MaxPlayers;
        public String LevelType;
        public VarInt ViewDistance;
        public Boolean ReducedDebugInfo;
        //public Boolean EnableRespawnScreen;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			GameMode = deserializer.Read(GameMode);
            Dimension = deserializer.Read(Dimension);
            //HashedSeed = deserializer.Read(HashedSeed);
            MaxPlayers = deserializer.Read(MaxPlayers);
            LevelType = deserializer.Read(LevelType);
            ViewDistance = deserializer.Read(ViewDistance);
            ReducedDebugInfo = deserializer.Read(ReducedDebugInfo);
            //EnableRespawnScreen = deserializer.Read(EnableRespawnScreen);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(GameMode);
            serializer.Write(Dimension);
            //serializer.Write(HashedSeed);
            serializer.Write(MaxPlayers);
            serializer.Write(LevelType);
            serializer.Write(ViewDistance);
            serializer.Write(ReducedDebugInfo);
            //serializer.Write(EnableRespawnScreen);
        }
    }
}