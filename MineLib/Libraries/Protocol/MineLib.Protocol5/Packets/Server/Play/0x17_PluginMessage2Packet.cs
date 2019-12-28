using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PluginMessage2Packet : ServerPlayPacket
    {
		public String Channel;
		public Byte[] Data;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Channel = deserialiser.Read(Channel);
			var DataLength = deserialiser.Read<Int16>();
			Data = deserialiser.Read(Data, DataLength);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Channel);
            serializer.Write((Int16) Data.Length);
            serializer.Write(Data, false);
        }
    }
}