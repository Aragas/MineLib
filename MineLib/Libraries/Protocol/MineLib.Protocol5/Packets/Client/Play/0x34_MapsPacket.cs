using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class MapsPacket : ClientPlayPacket
    {
		public VarInt ItemDamage;
		public Byte[] Data;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			ItemDamage = deserialiser.Read(ItemDamage);
			var DataLength = deserialiser.Read<Int16>();
			Data = deserialiser.Read(Data, DataLength);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(ItemDamage);
            serializer.Write((Int16) Data.Length);
            serializer.Write(Data, false);
        }
    }
}