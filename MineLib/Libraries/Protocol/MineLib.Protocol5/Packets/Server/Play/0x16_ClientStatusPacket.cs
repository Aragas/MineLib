using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class ClientStatusPacket : ServerPlayPacket
    {
		public SByte ActionID;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			ActionID = deserialiser.Read(ActionID);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(ActionID);
        }
    }
}