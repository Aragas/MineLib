using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class CloseWindow2Packet : ServerPlayPacket
    {
		public SByte WindowID;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			WindowID = deserialiser.Read(WindowID);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(WindowID);
        }
    }
}