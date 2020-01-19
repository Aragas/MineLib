using System;
using Aragas.Network.IO;

namespace MineLib.Protocol.Netty.Packets.Client.Login
{
    public class Disconnect2Packet : ClientLoginPacket
    {
		public String JSONData;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			JSONData = deserialiser.Read(JSONData);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
			serializer.Write(JSONData);
        }
    }
}