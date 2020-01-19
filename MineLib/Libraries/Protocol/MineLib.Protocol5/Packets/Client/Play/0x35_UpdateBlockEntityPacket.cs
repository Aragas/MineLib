using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class UpdateBlockEntityPacket : ClientPlayPacket
    {
		public Int32 X;
		public Int16 Y;
		public Int32 Z;
		public Byte Action;
		public Byte[] NBTData;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Action = deserializer.Read(Action);
            var NBTDataLength = deserializer.Read<Int16>();
            NBTData = deserializer.Read(NBTData, NBTDataLength);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Action);
            serializer.Write((Int16) NBTData.Length);
            serializer.Write(NBTData);
        }
    }
}