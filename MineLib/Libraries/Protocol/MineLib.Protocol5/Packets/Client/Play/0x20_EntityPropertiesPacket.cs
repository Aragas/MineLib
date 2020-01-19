using System;
using Aragas.Network.IO;
using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityPropertiesPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public EntityProperty[] Properties;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            EntityID = deserializer.Read(EntityID);
            var PropertiesLength = deserializer.Read<Int32>();
            Properties = deserializer.Read(Properties, PropertiesLength);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Properties.Length);
            serializer.Write(Properties, false);
        }
    }
}