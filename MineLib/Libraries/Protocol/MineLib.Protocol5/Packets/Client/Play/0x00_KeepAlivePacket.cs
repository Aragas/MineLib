using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class KeepAlivePacket : ClientPlayPacket
    {
		public Int32 KeepAliveID;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			KeepAliveID = deserialiser.Read(KeepAliveID);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(KeepAliveID);
        }
    }
}