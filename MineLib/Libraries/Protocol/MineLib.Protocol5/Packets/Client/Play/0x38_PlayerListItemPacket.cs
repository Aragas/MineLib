using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class PlayerListItemPacket : ClientPlayPacket
    {
		public String PlayerName;
		public Boolean Online;
		public Int16 Ping;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			PlayerName = deserializer.Read(PlayerName);
			Online = deserializer.Read(Online);
			Ping = deserializer.Read(Ping);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(PlayerName);
            serializer.Write(Online);
            serializer.Write(Ping);
        }
    }
}