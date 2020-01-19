using System;

using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Login
{
    public class SetCompressionPacket : ClientLoginPacket
    {
		public Int32 Threshold;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Threshold = deserializer.Read(Threshold);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Threshold);
        }
    }
}