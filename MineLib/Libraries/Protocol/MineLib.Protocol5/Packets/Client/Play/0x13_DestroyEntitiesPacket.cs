using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class DestroyEntitiesPacket : ClientPlayPacket
    {
		public Int32[] EntityIDs;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            var EntityIDsLength = deserializer.Read<Byte>();
            EntityIDs = deserializer.Read(EntityIDs, EntityIDsLength);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write((Byte) EntityIDs.Length);
            serializer.Write(EntityIDs, false);
        }

    }
}