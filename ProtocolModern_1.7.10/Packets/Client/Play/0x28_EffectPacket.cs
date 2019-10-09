using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EffectPacket : ProtobufPacket
    {
		public Int32 EffectID;
		public Int32 X;
		public SByte Y;
		public Int32 Z;
		public Int32 Data;
		public Boolean DisableRelativeVolume;

        public override VarInt ID => ClientPlayPacketTypes.Effect;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EffectID = reader.Read(EffectID);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Data = reader.Read(Data);
			DisableRelativeVolume = reader.Read(DisableRelativeVolume);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EffectID);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Data);
			stream.Write(DisableRelativeVolume);
          
            return this;
        }

    }
}