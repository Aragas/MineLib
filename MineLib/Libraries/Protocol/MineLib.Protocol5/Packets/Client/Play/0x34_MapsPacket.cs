using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class MapsPacket : ClientPlayPacket
    {
		public VarInt ItemDamage;
		public Byte[] Data;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			ItemDamage = deserializer.Read(ItemDamage);
			var DataLength = deserializer.Read<Int16>();
			Data = deserializer.Read(Data, DataLength);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ItemDamage);
            serializer.Write((Int16) Data.Length);
            serializer.Write(Data, false);
        }
    }
}