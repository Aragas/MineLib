using System;

using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Client.Login
{
    [Packet(0x04)]
    public class LoginPluginRequestPacket : ClientLoginPacket
    {
        public VarInt MessageID { get; set; }
        public String Channel { get; set; }
        public byte[] Data { get; set; }

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            MessageID = deserializer.Read(MessageID);
            Channel = deserializer.Read(Channel);
            Data = deserializer.Read(Data);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(MessageID);
            serializer.Write(Channel);
            serializer.Write(Data);
        }
    }
}