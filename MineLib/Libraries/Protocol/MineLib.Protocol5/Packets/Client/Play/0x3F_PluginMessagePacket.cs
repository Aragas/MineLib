using System;
using System.Text;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class PluginMessagePacket : ClientPlayPacket
    {
		public String Channel;
		public Byte[] Data;
        public string DataString => Encoding.UTF8.GetString(Data, 0, Data.Length);

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