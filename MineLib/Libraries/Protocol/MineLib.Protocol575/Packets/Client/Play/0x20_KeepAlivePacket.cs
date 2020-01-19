using System;

using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Play
{
    public class KeepAlivePacket : ClientPlayPacket
    {
		public Int64 KeepAliveID;

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