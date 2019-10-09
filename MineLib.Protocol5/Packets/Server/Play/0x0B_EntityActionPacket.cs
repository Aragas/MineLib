using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class EntityActionPacket : ServerPlayPacket
    {
		public Int32 EntityID;
		public SByte ActionID;
		public Int32 JumpBoost;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			ActionID = deserialiser.Read(ActionID);
			JumpBoost = deserialiser.Read(JumpBoost);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(ActionID);
            serializer.Write(JumpBoost);
        }
    }
}