using System;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Server.Play
{
    public class KeepAlive2Packet : ServerPlayPacket
    {
		public Int64 KeepAliveID;

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