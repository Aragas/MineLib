using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class RemoveEntityEffectPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte EffectID;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			EffectID = deserializer.Read(EffectID);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(EffectID);
        }
    }
}