using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class EntityActionPacket : ServerPlayPacket
    {
		public Int32 EntityID;
		public SByte ActionID;
		public Int32 JumpBoost;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			ActionID = deserializer.Read(ActionID);
			JumpBoost = deserializer.Read(JumpBoost);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(ActionID);
            serializer.Write(JumpBoost);
        }
    }
}