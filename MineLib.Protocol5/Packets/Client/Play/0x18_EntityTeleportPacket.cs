using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityTeleportPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public SByte Yaw;
		public SByte Pitch;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Yaw = deserialiser.Read(Yaw);
			Pitch = deserialiser.Read(Pitch);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
        }
    }
}