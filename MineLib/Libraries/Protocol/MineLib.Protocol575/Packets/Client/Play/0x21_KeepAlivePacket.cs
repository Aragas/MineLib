using System;

using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Play
{
    [PacketID(0x20)]
    public class KeepAlivePacket : ClientPlayPacket
    {
		public Int64 KeepAliveID;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			KeepAliveID = deserializer.Read(KeepAliveID);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(KeepAliveID);
        }
    }
}