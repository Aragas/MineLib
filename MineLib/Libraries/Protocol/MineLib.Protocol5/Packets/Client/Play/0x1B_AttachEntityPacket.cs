using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class AttachEntityPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Int32 VehicleID;
		public Boolean Leash;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			VehicleID = deserialiser.Read(VehicleID);
			Leash = deserialiser.Read(Leash);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(VehicleID);
            serializer.Write(Leash);
        }
    }
}