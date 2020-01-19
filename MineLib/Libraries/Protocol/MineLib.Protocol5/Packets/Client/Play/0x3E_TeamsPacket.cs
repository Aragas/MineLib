using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class TeamsPacket : ClientPlayPacket
    {
		public String TeamName;
		public SByte Mode;
		public String TeamDisplayName;
		public String TeamPrefix;
		public String TeamSuffix;
		public SByte FriendlyFire;
		//public NotSupportedType Players;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			TeamName = deserializer.Read(TeamName);
			Mode = deserializer.Read(Mode);
			TeamDisplayName = deserializer.Read(TeamDisplayName);
			TeamPrefix = deserializer.Read(TeamPrefix);
			TeamSuffix = deserializer.Read(TeamSuffix);
			FriendlyFire = deserializer.Read(FriendlyFire);
			var PlayersLength = deserializer.Read<short>();
			//Players = deserializer.Read(Players);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(TeamName);
            serializer.Write(Mode);
            serializer.Write(TeamDisplayName);
            serializer.Write(TeamPrefix);
            serializer.Write(TeamSuffix);
            serializer.Write(FriendlyFire);
			//stream.Write((short) Players.Length);
			//stream.Write(Players);
        }
    }
}