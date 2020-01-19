using System;

using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Login
{
    public class Disconnect2Packet : ClientLoginPacket
    {
		public String JSONData;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			JSONData = deserializer.Read(JSONData);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
			serializer.Write(JSONData);
        }
    }
}