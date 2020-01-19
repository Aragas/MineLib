using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EffectPacket : ClientPlayPacket
    {
		public Int32 EffectID;
		public Int32 X;
		public SByte Y;
		public Int32 Z;
		public Int32 Data;
		public Boolean DisableRelativeVolume;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EffectID = deserializer.Read(EffectID);
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Data = deserializer.Read(Data);
			DisableRelativeVolume = deserializer.Read(DisableRelativeVolume);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EffectID);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Data);
            serializer.Write(DisableRelativeVolume);
        }
    }
}