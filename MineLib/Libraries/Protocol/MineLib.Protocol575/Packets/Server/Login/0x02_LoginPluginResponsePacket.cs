using System;

using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol575.Packets.Server.Login
{
    public class LoginPluginResponsePacket : ServerLoginPacket
    {
        public VarInt MessageID { get; set; }
        public Boolean Successful { get; set; }
        public byte[] Data { get; set; }

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            MessageID = deserialiser.Read(MessageID);
            Successful = deserialiser.Read(Successful);
            Data = deserialiser.Read(Data);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(MessageID);
            serializer.Write(Successful);
            serializer.Write(Data);
        }
    }
}