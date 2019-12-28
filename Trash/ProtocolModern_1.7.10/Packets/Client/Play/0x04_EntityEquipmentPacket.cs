using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using MineLib.Core.Data;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityEquipmentPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public Int16 Slot;
		public ItemStack Item;

        public override VarInt ID => ClientPlayPacketTypes.EntityEquipment;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Slot = reader.Read(Slot);
			//Item = reader.Read(Item);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Slot);
			stream.Write(Item);
          
            return this;
        }

    }
}