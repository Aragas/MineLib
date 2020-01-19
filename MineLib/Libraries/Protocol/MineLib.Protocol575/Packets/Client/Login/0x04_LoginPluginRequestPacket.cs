using System;

using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Login
{
    public class LoginPluginRequestPacket : ClientLoginPacket
    {
        public VarInt MessageID { get; set; }
        public String Channel { get; set; }
        public byte[] Data { get; set; }

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
            MessageID = deserialiser.Read(MessageID);
            Channel = deserialiser.Read(Channel);
            Data = deserialiser.Read(Data);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(MessageID);
            serializer.Write(Channel);
            serializer.Write(Data);
        }
    }
}