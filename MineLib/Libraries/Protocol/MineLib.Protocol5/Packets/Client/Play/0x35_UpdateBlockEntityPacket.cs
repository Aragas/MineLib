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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Action = deserialiser.Read(Action);
            var NBTDataLength = deserialiser.Read<Int16>();
            NBTData = deserialiser.Read(NBTData, NBTDataLength);
        }

        public override void Serialize(IStreamSerializer serializer)
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