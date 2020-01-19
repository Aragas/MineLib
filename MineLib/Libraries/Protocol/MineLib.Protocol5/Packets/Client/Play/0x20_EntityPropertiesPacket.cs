using System;
using Aragas.Network.IO;
using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityPropertiesPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public EntityProperty[] Properties;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
            EntityID = deserialiser.Read(EntityID);
            var PropertiesLength = deserialiser.Read<Int32>();
            Properties = deserialiser.Read(Properties, PropertiesLength);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Properties.Length);
            serializer.Write(Properties, false);
        }
    }
}