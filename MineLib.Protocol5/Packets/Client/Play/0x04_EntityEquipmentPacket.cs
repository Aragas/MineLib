using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityEquipmentPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Int16 Slot;
		//public ItemStack Item; // TODO:

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			Slot = deserialiser.Read(Slot);
			//Item = deserialiser.Read(Item);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Slot);
            //serializer.Write(Item);
        }
    }
}