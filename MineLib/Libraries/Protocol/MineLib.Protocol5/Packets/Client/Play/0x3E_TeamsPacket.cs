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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			TeamName = deserialiser.Read(TeamName);
			Mode = deserialiser.Read(Mode);
			TeamDisplayName = deserialiser.Read(TeamDisplayName);
			TeamPrefix = deserialiser.Read(TeamPrefix);
			TeamSuffix = deserialiser.Read(TeamSuffix);
			FriendlyFire = deserialiser.Read(FriendlyFire);
			var PlayersLength = deserialiser.Read<short>();
			//Players = deserialiser.Read(Players);
        }

        public override void Serialize(IStreamSerializer serializer)
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