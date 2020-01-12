using System;

using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Login
{
    public class SetCompressionPacket : ClientLoginPacket
    {
		public Int32 Threshold;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            Threshold = deserialiser.Read(Threshold);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Threshold);
        }
    }
}