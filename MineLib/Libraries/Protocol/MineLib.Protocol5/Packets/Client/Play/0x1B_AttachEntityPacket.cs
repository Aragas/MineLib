using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class AttachEntityPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Int32 VehicleID;
		public Boolean Leash;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			VehicleID = deserializer.Read(VehicleID);
			Leash = deserializer.Read(Leash);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(VehicleID);
            serializer.Write(Leash);
        }
    }
}