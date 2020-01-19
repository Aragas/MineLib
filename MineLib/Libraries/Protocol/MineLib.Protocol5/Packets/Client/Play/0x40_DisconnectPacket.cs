using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class DisconnectPacket : ClientPlayPacket
    {
		public String Reason;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Reason = deserialiser.Read(Reason);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Reason);
        }
    }
}