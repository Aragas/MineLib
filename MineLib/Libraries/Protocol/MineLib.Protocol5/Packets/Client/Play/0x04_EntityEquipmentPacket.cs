using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityEquipmentPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Int16 Slot;
		//public ItemStack Item; // TODO:

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			Slot = deserializer.Read(Slot);
			//Item = deserializer.Read(Item);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Slot);
            //serializer.Write(Item);
        }
    }
}