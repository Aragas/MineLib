﻿using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Overworld
{
    public class DisconnectPacket : PokeDPacket
    {
        public string Reason { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Reason = deserializer.Read(Reason);
        }
        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Reason);
        }
    }
}
