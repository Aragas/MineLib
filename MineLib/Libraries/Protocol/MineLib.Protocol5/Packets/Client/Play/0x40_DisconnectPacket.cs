using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class DisconnectPacket : ClientPlayPacket
    {
		public String Reason;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Reason = deserializer.Read(Reason);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Reason);
        }
    }
}