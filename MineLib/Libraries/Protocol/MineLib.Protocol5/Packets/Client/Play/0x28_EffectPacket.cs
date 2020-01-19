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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EffectID = deserialiser.Read(EffectID);
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Data = deserialiser.Read(Data);
			DisableRelativeVolume = deserialiser.Read(DisableRelativeVolume);
        }

        public override void Serialize(IStreamSerializer serializer)
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