using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class ClientStatusPacket : ServerPlayPacket
    {
		public SByte ActionID;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			ActionID = deserializer.Read(ActionID);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ActionID);
        }
    }
}