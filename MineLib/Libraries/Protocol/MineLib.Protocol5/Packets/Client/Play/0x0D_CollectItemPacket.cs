using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class CollectItemPacket : ClientPlayPacket
    {
		public Int32 CollectedEntityID;
		public Int32 CollectorEntityID;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			CollectedEntityID = deserialiser.Read(CollectedEntityID);
			CollectorEntityID = deserialiser.Read(CollectorEntityID);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(CollectedEntityID);
            serializer.Write(CollectorEntityID);
        }
    }
}