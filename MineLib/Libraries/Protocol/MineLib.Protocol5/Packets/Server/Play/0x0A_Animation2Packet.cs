using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class Animation2Packet : ServerPlayPacket
    {
		public Int32 EntityID;
		public SByte Animation;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			Animation = deserialiser.Read(Animation);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Animation);
        }
    }
}