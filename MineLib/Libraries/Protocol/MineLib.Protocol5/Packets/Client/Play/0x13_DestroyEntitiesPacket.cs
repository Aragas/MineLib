using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class DestroyEntitiesPacket : ClientPlayPacket
    {
		public Int32[] EntityIDs;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            var EntityIDsLength = deserialiser.Read<Byte>();
            EntityIDs = deserialiser.Read(EntityIDs, EntityIDsLength);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write((Byte) EntityIDs.Length);
            serializer.Write(EntityIDs, false);
        }

    }
}