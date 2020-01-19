using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PluginMessage2Packet : ServerPlayPacket
    {
		public String Channel;
		public Byte[] Data;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Channel = deserializer.Read(Channel);
			var DataLength = deserializer.Read<Int16>();
			Data = deserializer.Read(Data, DataLength);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Channel);
            serializer.Write((Int16) Data.Length);
            serializer.Write(Data, false);
        }
    }
}