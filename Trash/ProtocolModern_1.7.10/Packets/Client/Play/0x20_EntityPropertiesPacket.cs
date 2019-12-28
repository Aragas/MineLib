using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Data;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityPropertiesPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public EntityProperty[] Properties;

        public override VarInt ID => ClientPlayPacketTypes.EntityProperties;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			//EntityID = reader.Read(EntityID);
            //var PropertiesLength = reader.Read<Int32>();
            //Properties = reader.Read(Properties, PropertiesLength);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
            stream.Write(EntityID);
            stream.Write(Properties.Length);
            stream.Write(Properties, false);
          
            return this;
        }

    }
}