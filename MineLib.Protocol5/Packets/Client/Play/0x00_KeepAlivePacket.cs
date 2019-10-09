using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class KeepAlivePacket : ClientPlayPacket
    {
		public Int32 KeepAliveID;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			KeepAliveID = deserialiser.Read(KeepAliveID);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(KeepAliveID);
        }
    }
}