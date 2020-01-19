using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class PlayerListItemPacket : ClientPlayPacket
    {
		public String PlayerName;
		public Boolean Online;
		public Int16 Ping;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			PlayerName = deserialiser.Read(PlayerName);
			Online = deserialiser.Read(Online);
			Ping = deserialiser.Read(Ping);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(PlayerName);
            serializer.Write(Online);
            serializer.Write(Ping);
        }
    }
}