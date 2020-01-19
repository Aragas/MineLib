using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class RemoveEntityEffectPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte EffectID;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			EffectID = deserialiser.Read(EffectID);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(EffectID);
        }
    }
}