using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class CollectItemPacket : ProtobufPacket
    {
		public Int32 CollectedEntityID;
		public Int32 CollectorEntityID;

        public override VarInt ID => ClientPlayPacketTypes.CollectItem;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			CollectedEntityID = reader.Read(CollectedEntityID);
			CollectorEntityID = reader.Read(CollectorEntityID);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(CollectedEntityID);
			stream.Write(CollectorEntityID);
          
            return this;
        }

    }
}