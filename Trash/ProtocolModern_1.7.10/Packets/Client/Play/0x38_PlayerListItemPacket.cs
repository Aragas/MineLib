using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class PlayerListItemPacket : ProtobufPacket
    {
		public String PlayerName;
		public Boolean Online;
		public Int16 Ping;

        public override VarInt ID => ClientPlayPacketTypes.PlayerListItem;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			PlayerName = reader.Read(PlayerName);
			Online = reader.Read(Online);
			Ping = reader.Read(Ping);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(PlayerName);
			stream.Write(Online);
			stream.Write(Ping);
          
            return this;
        }

    }
}